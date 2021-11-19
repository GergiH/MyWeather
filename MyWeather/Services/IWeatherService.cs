using MyWeather.Data;
using MyWeather.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Services
{
    public interface IWeatherService
    {
        Task<CityForecast> GetDailyForecastsAsync(int cityId, string cityName, bool isFavourite = false);
        Task<City> GetCityByIdAsync(int cityId);
        Task<List<City>> GetCitiesAsync();
        Task<UserCity> GetUserCityAsync(ApplicationUser user, int cityId);
        Task<List<UserCity>> GetUserCitiesAsync(ApplicationUser user);
        Task<UserCity> AddOrUpdateUserCityAsync(ApplicationUser user, int cityId, bool isFavourite, int newCityId = -1);
        Task<bool> DeleteUserCityAsync(ApplicationUser user, int cityToDelete);
    }
}
