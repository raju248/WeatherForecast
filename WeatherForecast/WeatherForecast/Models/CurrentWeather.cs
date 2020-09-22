using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeatherForecast.Models
{
    public class CurrentWeather
    {
        [Required, ForeignKey("City")]
        public int Id { get; set; }
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public double uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public int weather_id { get; set; }
        public string weather_main { get; set; }
        public string weather_description { get; set; }
        public string weather_icon { get; set; }


        [JsonIgnore]
        [Required]
        public virtual City City { get; set; }
    }


}