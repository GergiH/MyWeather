using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeatherData.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public List<UserCity> UserCities { get; set; }
    }
}
