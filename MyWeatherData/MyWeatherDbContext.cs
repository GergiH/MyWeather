using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MyWeatherData.Entities;

namespace MyWeatherData
{
    public class MyWeatherDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public MyWeatherDbContext(DbContextOptions<MyWeatherDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<UserCity> UserCities { get; set; }
        public DbSet<Forecast> Forecasts { get; set; }
        public DbSet<Weather> Weathers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Setting composite keys with annotations is not possible
            modelBuilder.Entity<Forecast>()
                .HasKey(f => new { f.ValidDate, f.CityId });
            modelBuilder.Entity<Weather>()
                .HasKey(w => new { w.ValidDate, w.CityId });
            modelBuilder.Entity<ApplicationUser>()
                .HasMany<UserCity>();
            modelBuilder.Entity<UserCity>()
                .HasKey(u => new { u.UserId, u.CityId });
        }
    }
}
