using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherForecast.Models
{
    public class DailyForecast
    {
        public int DailyForecastId { set; get; }
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public double temp_day { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double temp_night { get; set; }
        public double temp_eve { get; set; }
        public double morn { get; set; }
        public double feels_like_day { get; set; }
        public double feels_like_night { get; set; }
        public double feels_like_eve { get; set; }
        public double feels_like_morn { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public int weather_id { get; set; }
        public string weather_main { get; set; }
        public string weather_description { get; set; }
        public string weather_icon { get; set; }
        public int clouds { get; set; }
        public double uvi { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        [JsonIgnore]
        public virtual City City { get; set; }
    }
}