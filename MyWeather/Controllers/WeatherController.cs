using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWeather.Data;
using MyWeather.Models;
using MyWeather.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Controllers
{
    [Authorize]  // Could use [Authorize] per function if needed, but here we just tie everything related to Weather
    public class WeatherController : Controller
    {
        private IWeatherService _service;
        private UserManager<ApplicationUser> _userManager;
        private MyWeatherDbContext _context;

        public WeatherController(IWeatherService service, UserManager<ApplicationUser> userManager, MyWeatherDbContext context)
        {
            _service = service;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ForecastViewModel fcViewModel = new ForecastViewModel();

            fcViewModel = await HandleForecastVMLists(fcViewModel, user);

            return View(fcViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddCityForecast([FromForm] ForecastViewModel fcViewModel)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            City selectedCity = await _service.GetCityByIdAsync(fcViewModel.SelectedCityId);
            if (!fcViewModel.UserCities.Any(c => c.CityId == selectedCity.CityId))
            {
                fcViewModel.UserCities.Add(new UserCity
                {
                    City = selectedCity,
                    CityId = selectedCity.CityId,
                    UserId = user.Id
                });
                await _service.AddOrUpdateUserCityAsync(user, fcViewModel.SelectedCityId, false);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> FavouriteCityForecast(int cityToFavourite)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            UserCity userCity = await _service.GetUserCityAsync(user, cityToFavourite);

            return View(userCity);
        }

        [HttpPost]
        public async Task<IActionResult> FavouriteCityForecastConfirmed(UserCity userCity)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            // Inverting IsFavourite so it's good for un-favouriting and favouriting Cities
            await _service.AddOrUpdateUserCityAsync(user, userCity.CityId, !userCity.IsFavourite);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCityForecast(int cityToUpdate)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            UserCity userCity = await _service.GetUserCityAsync(user, cityToUpdate);
            List<SelectListItem> cityList = new(await Globals.GetCitySelectListAsync(_context));
            ViewData["CityList"] = cityList;

            return View(userCity);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCityForecastConfirmed(UserCity userCity, int newCityId)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            await _service.AddOrUpdateUserCityAsync(user, userCity.CityId, userCity.IsFavourite, newCityId);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCityForecast(int cityToDelete)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            UserCity userCity = await _service.GetUserCityAsync(user, cityToDelete);

            return View(userCity);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCityForecastConfirmed(UserCity userCity)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            await _service.DeleteUserCityAsync(user, userCity.CityId);

            return RedirectToAction("Index");
        }

        public async Task<ForecastViewModel> HandleForecastVMLists(ForecastViewModel fcViewModel, ApplicationUser user)
        {
            fcViewModel.CityList = new(await Globals.GetCitySelectListAsync(_context));
            fcViewModel.UserCities = await _service.GetUserCitiesAsync(user);

            if (fcViewModel.UserCities.Count != 0 && fcViewModel.CityForecasts.Count == 0)
            {
                foreach (var uc in fcViewModel.UserCities)
                {
                    fcViewModel.CityForecasts.Add(await _service.GetDailyForecastsAsync(uc.CityId, uc.City.CityName, uc.IsFavourite));
                }

                fcViewModel.CityForecasts = fcViewModel.CityForecasts.OrderBy(c => !c.IsFavourite).ToList();
            }

            // Take out the selected UserCities from the dropdown as they should not be (re)selectable
            fcViewModel.CityList.RemoveAll(c => fcViewModel.UserCities.Any(u => u.CityId.ToString() == c.Value));

            return fcViewModel;
        }
    }
}
