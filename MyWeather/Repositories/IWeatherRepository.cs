using MyWeatherData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Repositories
{
    public interface IWeatherRepository : IDisposable
    {
        Task<List<Forecast>> GetForecastsFromDbAsync(int cityId);
        Task<List<Forecast>> GetDailyForecastsAsync(int cityId);
        Task<UserCity> GetUserCityAsync(ApplicationUser user, int cityId);
        Task<List<UserCity>> GetUserCitiesAsync(ApplicationUser user);
        Task<City> GetCityByIdAsync(int cityId);
        Task<List<City>> GetCitiesAsync();
        Task<UserCity> AddOrUpdateUserCityAsync(ApplicationUser user, int cityId, bool isFavourite, int newCityId = -1);
        Task<bool> DeleteUserCityAsync(ApplicationUser user, int cityId);
    }
}
