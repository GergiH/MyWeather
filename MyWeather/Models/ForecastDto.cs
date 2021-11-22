using MyWeatherData.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Models
{
    public class ForecastDto
    {
        [JsonProperty("data")]
        public List<Forecast> Forecasts { get; set; }
    }
}
