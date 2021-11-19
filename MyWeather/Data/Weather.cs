using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Data
{
	public class Weather
	{
		public string Code { get; set; }
		public string Icon { get; set; }
		public string Description { get; set; }
		public string ValidDate { get; set; }
		public int CityId { get; set; }
	}
}
