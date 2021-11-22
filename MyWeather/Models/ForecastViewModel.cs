using Microsoft.AspNetCore.Mvc.Rendering;
using MyWeatherData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Models
{
    public class ForecastDisplay
    {
        public DateTime WeatherDate { get; set; }
        public double? AvgTemp { get; set; }
        public double? MaxTemp { get; set; }
        public double? MinTemp { get; set; }
        public string Description { get; set; }
        public double? PrecProbability { get; set; }
    }

    public class CityForecast
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public bool IsFavourite { get; set; }
        public List<ForecastDisplay> ForecastDisplays { get; set; }
    }

    public class ForecastViewModel
    {
        public int SelectedCityId { get; set; }
        public List<SelectListItem> CityList { get; set; } = new List<SelectListItem>();
        public List<UserCity> UserCities { get; set; } = new List<UserCity>();
        public List<CityForecast> CityForecasts { get; set; } = new List<CityForecast>();
    }
}
