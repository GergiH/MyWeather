using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyWeather.Data
{
	public class Forecast
	{
		[JsonProperty("valid_date")]
		public string ValidDate { get; set; }
		[JsonProperty("moonrise_ts")]
		public string MoonriseTS { get; set; }
		[JsonProperty("wind_cdir")]
		public string WindCdir { get; set; }
		[JsonProperty("rh")]
		public double? RH { get; set; }
		[JsonProperty("pres")]
		public double? Pres { get; set; }
		[JsonProperty("high_temp")]
		public double? HighTemp { get; set; }
		[JsonProperty("sunset_ts")]
		public string SunsetTS { get; set; }
		[JsonProperty("ozone")]
		public double? Ozone { get; set; }
		[JsonProperty("moon_phase")]
		public double? MoonPhase { get; set; }
		[JsonProperty("wind_gust_spd")]
		public double? WindGustSpd { get; set; }
		[JsonProperty("snow_depth")]
		public double? SnowDepth { get; set; }
		[JsonProperty("clouds")]
		public double? Clouds { get; set; }
		[JsonProperty("ts")]
		public string TS { get; set; }
		[JsonProperty("sunrise_ts")]
		public string SunriseTS { get; set; }
		[JsonProperty("app_min_temp")]
		public double? AppMinTemp { get; set; }
		[JsonProperty("wind_spd")]
		public double? WindSpd { get; set; }
		[JsonProperty("pop")]
		public double? Pop { get; set; }
		[JsonProperty("wind_cdir_full")]
		public string WindCdirFull { get; set; }
		[JsonProperty("slp")]
		public double? SLP { get; set; }
		[JsonProperty("moon_phase_lunation")]
		public double? MoonPhaseLunation { get; set; }
		[JsonProperty("app_max_temp")]
		public double? AppMaxTemp { get; set; }
		[JsonProperty("vis")]
		public double? VIS { get; set; }
		[JsonProperty("dewpt")]
		public double? DEWPT { get; set; }
		[JsonProperty("snow")]
		public double? Snow { get; set; }
		[JsonProperty("uv")]
		public double? UV { get; set; }
		[JsonProperty("wind_dir")]
		public double? WindDir { get; set; }
		[JsonProperty("max_dhi")]
		public double? MaxDHI { get; set; }
		[JsonProperty("clouds_hi")]
		public double? CloudsHI { get; set; }
		[JsonProperty("precip")]
		public double? Precip { get; set; }
		[JsonProperty("low_temp")]
		public double? LowTemp { get; set; }
		[JsonProperty("max_temp")]
		public double? MaxTemp { get; set; }
		[JsonProperty("moonset_ts")]
		public string MoonsetTS { get; set; }
		[JsonProperty("datetime")]
		public string Datetime { get; set; }
		[JsonProperty("temp")]
		public double? Temp { get; set; }
		[JsonProperty("min_temp")]
		public double? MinTemp { get; set; }
		[JsonProperty("clouds_mid")]
		public double? CloudsMid { get; set; }
		[JsonProperty("clouds_low")]
		public double? CloudsLow { get; set; }
		[JsonProperty("weather")]
		public Weather Weather { get; set; }
        public int CityId { get; set; }
        [ForeignKey("CityId")]
		public City City { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
