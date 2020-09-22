using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WeatherForecast.Models;

namespace WeatherForecast
{
    public class WeatherForecastDBContext : DbContext
    {

        public WeatherForecastDBContext() : base("WeatherForecastDBContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           // modelBuilder.Entity<City>().HasOptional(c => c.currentWeather).WithRequired(cw => cw.City);
        }


        public DbSet<City> city { get; set; }
        public DbSet<CurrentWeather> currentWeathers { get; set; }
        public DbSet<DailyForecast> dailyForecasts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Blog> Blogs { get; set; }
    }
}