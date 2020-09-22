using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherForecast.Models
{
    public class City
    {
        // [Required]
        [Key]
        public int CityId { set; get; }
        public string City_Name { set; get; }
        public double Latitude { set; get; }
        public double Longitude { set; get; }
        public string Country { get; set; }

        public virtual CurrentWeather currentWeather { get; set; }
        public virtual List<DailyForecast> dailyForcasts { get; set; }
    }

}