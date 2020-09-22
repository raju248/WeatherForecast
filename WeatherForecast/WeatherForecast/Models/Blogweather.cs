using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherForecast.Models
{
    public class Blogweather
    {
        public List<Blog> Blogs { get; set; }
        public List<CurrentWeather> CurrentWeathers { get; set; }

    }
}