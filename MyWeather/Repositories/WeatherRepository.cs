using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyWeather.Models;
using MyWeatherData;
using MyWeatherData.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyWeather.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private ILogger<WeatherRepository> _log;
        private MyWeatherDbContext _context;
        private string _baseUrl;
        private string _weatherBitApiKey;
        private HttpClient _client = new HttpClient();

        public WeatherRepository(IConfiguration config, ILogger<WeatherRepository> log, MyWeatherDbContext context)
        {
            _log = log;
            _context = context;
            _baseUrl = config.GetValue<string>("WeatherBitBaseUrl");
            _weatherBitApiKey = config.GetValue<string>("WeatherBitApiKey");
        }

        public async Task<UserCity> GetUserCityAsync(ApplicationUser user, int cityId)
        {
            return await _context.UserCities
                .Include(u => u.City)
                .FirstOrDefaultAsync(u => u.UserId == user.Id && u.CityId == cityId);
        }

        public async Task<List<UserCity>> GetUserCitiesAsync(ApplicationUser user)
        {
            return await _context.UserCities
                .Include(u => u.City)
                .Where(u => u.UserId == user.Id)
                .OrderBy(u => u.City.CityName)
                .ToListAsync();
        }

        public async Task<City> GetCityByIdAsync(int cityId)
        {
            return await _context.Cities.FindAsync(cityId);
        }

        public async Task<List<City>> GetCitiesAsync()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<UserCity> AddOrUpdateUserCityAsync(ApplicationUser user, int cityId, bool isFavourite, int newCityId = -1)
        {
            UserCity userCity = await _context.UserCities.FirstOrDefaultAsync(u => u.UserId == user.Id && u.CityId == cityId);
            if (userCity == null)
            {
                userCity = new UserCity
                {
                    CityId = cityId,
                    UserId = user.Id,
                    IsFavourite = isFavourite
                };
                await _context.UserCities.AddAsync(userCity);
            }
            else
            {
                if (newCityId != -1)
                {
                    UserCity newUC = new UserCity
                    {
                        CityId = newCityId,
                        IsFavourite = userCity.IsFavourite,
                        UserId = user.Id
                    };
                    _context.UserCities.Remove(userCity);
                    await _context.UserCities.AddAsync(newUC);
                }
                else
                {
                    userCity.IsFavourite = isFavourite;
                    _context.UserCities.Update(userCity);
                }
            }
            await _context.SaveChangesAsync();

            return userCity;
        }

        public async Task<bool> DeleteUserCityAsync(ApplicationUser user, int cityId)
        {
            UserCity userCity = await _context.UserCities.FirstOrDefaultAsync(u => u.UserId == user.Id && u.CityId == cityId);
            if (userCity != null)
            {
                _context.UserCities.Remove(userCity);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<List<Forecast>> GetForecastsFromDbAsync(int cityId)
        {
            return await _context.Forecasts
                .Include(f => f.Weather)
                .Where(f => f.CityId == cityId)
                .OrderByDescending(f => f.ModifiedAt)
                .ToListAsync();
        }

        public async Task<List<Forecast>> GetDailyForecastsAsync(int cityId)
        {
            string filters = $"&city_id={cityId}";
            string forecastUrl = $"{_baseUrl}/forecast/daily?key={_weatherBitApiKey}{filters}";
            ForecastDto forecastDto = new ForecastDto();

            // Get the Forecasts from the DB first and check if they have been updated in the last 2 hours, if yes
            // then return those, otherwise hit the API and get fresh data
            forecastDto.Forecasts = await GetForecastsFromDbAsync(cityId);
            if (forecastDto.Forecasts.Count > 0 && (DateTime.Now - forecastDto.Forecasts[0].ModifiedAt).TotalHours < 2)
            {
                return forecastDto.Forecasts;
            }

            try
            {
                using (HttpResponseMessage res = await _client.GetAsync(forecastUrl))
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        string resJsonString = await res.Content.ReadAsStringAsync();
                        forecastDto = JsonConvert.DeserializeObject<ForecastDto>(resJsonString);

                        forecastDto.Forecasts = forecastDto.Forecasts.OrderBy(f => DateTime.ParseExact(f.ValidDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();

                        foreach (Forecast fc in forecastDto.Forecasts)
                        {
                            Forecast fcInDb = await _context.Forecasts
                                .Include(f => f.Weather)
                                .FirstOrDefaultAsync(f => f.ValidDate == fc.ValidDate && f.CityId == cityId);

                            if (fcInDb == null)
                            {
                                fc.CityId = cityId;
                                fc.ModifiedAt = DateTime.Now;
                                fc.Weather.ValidDate = fc.ValidDate;
                                fc.Weather.CityId = cityId;
                                await _context.Forecasts.AddAsync(fc);
                                await _context.Weathers.AddAsync(fc.Weather);
                            }
                            else
                            {
                                fcInDb = fc;
                                fcInDb.CityId = cityId;
                                fcInDb.ModifiedAt = DateTime.Now;
                                fcInDb.Weather.ValidDate = fcInDb.ValidDate;
                                fcInDb.Weather.CityId = cityId;
                                _context.Forecasts.Update(fcInDb);
                                _context.Weathers.Update(fcInDb.Weather);
                            }
                        }
                        await _context.SaveChangesAsync();

                        return forecastDto.Forecasts;
                    }
                    else
                    {
                        return await _context.Forecasts
                            .Where(f => DateTime.ParseExact(f.ValidDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.Today)
                            .ToListAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
            }

            return forecastDto.Forecasts;
        }

        public void Dispose()
        {
        }
    }
}
