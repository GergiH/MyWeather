using CsvHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Data.Seed
{
    public class InitialSeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MyWeatherDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MyWeatherDbContext>>()))
            {
                bool addAll = false;
                int maxRecordCount = 100;

                bool doesSeed = false;
                string basePath = "Data\\Seed\\WeatherBitData\\";

                if (!context.Cities.Any())
                {
                    doesSeed = true;

                    using (StreamReader reader = new StreamReader($"{basePath}cities_all.csv"))
                    using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        IEnumerable<City> records = csv.GetRecords<City>();
                        if (addAll)
                        {
                            context.Cities.AddRange(records);
                        }
                        else
                        {
                            context.Cities.AddRange(records.Take(maxRecordCount));
                        }

                    }
                }

                if (doesSeed)
                {
                    context.SaveChanges();
                }
            }
        }
    }
}
