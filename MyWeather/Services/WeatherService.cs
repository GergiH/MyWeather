using Microsoft.Extensions.Logging;
using MyWeather.Models;
using MyWeather.Repositories;
using MyWeatherData.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Services
{
    public class WeatherService : IWeatherService
    {
        private ILogger<WeatherService> _log;
        private IWeatherRepository _repo;

        public WeatherService(ILogger<WeatherService> log, IWeatherRepository repo)
        {
            _log = log;
            _repo = repo;
        }

        public async Task<City> GetCityByIdAsync(int cityId)
        {
            return await _repo.GetCityByIdAsync(cityId);
        }

        public async Task<List<City>> GetCitiesAsync()
        {
            return await _repo.GetCitiesAsync();
        }

        public async Task<UserCity> GetUserCityAsync(ApplicationUser user, int cityId)
        {
            return await _repo.GetUserCityAsync(user, cityId);
        }

        public async Task<List<UserCity>> GetUserCitiesAsync(ApplicationUser user)
        {
            return await _repo.GetUserCitiesAsync(user);
        }

        public async Task<UserCity> AddOrUpdateUserCityAsync(ApplicationUser user, int cityId, bool isFavourite, int newCityId = -1)
        {
            return await _repo.AddOrUpdateUserCityAsync(user, cityId, isFavourite, newCityId);
        }

        public async Task<bool> DeleteUserCityAsync(ApplicationUser user, int cityToDelete)
        {
            return await _repo.DeleteUserCityAsync(user, cityToDelete);
        }

        public async Task<CityForecast> GetDailyForecastsAsync(int cityId, string cityName, bool isFavourite = false)
        {
            CityForecast cityForecast = new CityForecast
            {
                ForecastDisplays = new List<ForecastDisplay>()
            };

            try
            {
                List<Forecast> forecasts = await _repo.GetDailyForecastsAsync(cityId);

                List<ForecastDisplay> fcDisplays = new List<ForecastDisplay>();
                foreach (Forecast fc in forecasts)
                {
                    // Could use AutoMapper but for 1 class I didn't find it necessary
                    fcDisplays.Add(new ForecastDisplay
                    {
                        AvgTemp = fc.Temp,
                        MaxTemp = fc.MaxTemp,
                        MinTemp = fc.MinTemp,
                        Description = fc.Weather?.Description ?? "",
                        PrecProbability = fc.Pop,
                        WeatherDate = DateTime.ParseExact(fc.ValidDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    });
                }

                // After coverted to DateTime order by it
                fcDisplays = fcDisplays.OrderBy(f => f.WeatherDate).ToList();

                cityForecast.CityId = cityId;
                cityForecast.CityName = cityName;
                cityForecast.IsFavourite = isFavourite;
                cityForecast.ForecastDisplays = fcDisplays;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
            }

            return cityForecast;
        }
    }
}
