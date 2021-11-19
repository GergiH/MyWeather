using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Data
{
    public static class Globals
    {
        public static List<SelectListItem> CitySelectList = new List<SelectListItem>();
        public static async Task<List<SelectListItem>> GetCitySelectListAsync(MyWeatherDbContext context)
        {
            if (CitySelectList.Count == 0)
            {
                CitySelectList = await context.Cities
                    .OrderBy(c => c.CityName)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CityId.ToString(),
                        Text = c.CityName
                    })
                    .ToListAsync();
            }

            return CitySelectList;
        }
    }
}
