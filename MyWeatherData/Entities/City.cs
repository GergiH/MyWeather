using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeatherData.Entities
{
    public class City
    {
        [Key]
        [Name("city_id")]
        public int CityId { get; set; }
        [Name("city_name")]
        public string CityName { get; set; }
        [Name("lat")]
        public double Latitude { get; set; }
        [Name("lon")]
        public double Longitude { get; set; }
    }
}
