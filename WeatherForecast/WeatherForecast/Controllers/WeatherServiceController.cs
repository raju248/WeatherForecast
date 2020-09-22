using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeatherForecast.Models;
using PagedList;
using PagedList.Mvc;

namespace WeatherForecast.Controllers
{
    public class WeatherServiceController : Controller
    {
        string openWeatherMap_api_key = "f1a0cb5c07f966de4a22dbaa76167db6";
        string geoCoding_api_key = "AIzaSyD-gdavNyNOINU4YxkGE3Iy8bcDwQZAXP4";

        string[] district =
            { "Bagerhat", "Bandarban", "Barguna", "Barisal", "Bhola", "Bogura", "Brahmanbaria",
            "Chandpur", "Chapainawabganj", "Chattogram", "Chuadanga", "Comilla", "Coxsbazar", "Dhaka",
            "Dinajpur", "Faridpur", "Feni", "Gaibandha", "Gazipur", "Gopalganj", "Habiganj", "Jamalpur",
            "Jashore", "Jhalakathi", "Jhenaidah", "Joypurhat", "Khagrachhari", "Khulna", "Kishoreganj",
            "Kurigram", "Kushtia", "Lakshmipur", "Lalmonirhat", "Madaripur", "Magura", "Manikganj",
            "Meherpur", "Moulvibazar", "Munshiganj", "Mymensingh", "Naogaon", "Narail", "Narayanganj",
            "Narsingdi", "Natore", "Netrokona", "Nilphamari", "Noakhali", "Pabna", "Panchagarh", "Patuakhali",
            "Pirojpur", "Rajbari", "Rajshahi", "Rangamati", "Rangpur", "Satkhira", "Shariatpur", "Sherpur",
            "Sirajganj", "Sunamganj", "Sylhet", "Tangail", "Thakurgaon" };

        WeatherForecastDBContext db;

        public WeatherServiceController()
        {
            db = new WeatherForecastDBContext();
        }

        // GET: WeatherService
        public async Task<ActionResult> Index()
        {

            /*foreach (string s in district)
            {
                Location location = await getLocation(s + ",Bangladesh");

                db.city.Add(new City { City_Name = s, Country = "Bangladesh", Latitude = location.lat, Longitude = location.lng });
                db.SaveChanges();
            }*/
            // await Update();

            var modelblog = db.Blogs.ToList() ;
            var majorCitiesWeather = new List<CurrentWeather> ();

            

            majorCitiesWeather.Add(db.currentWeathers.Include(c => c.City).Where(c => c.City.City_Name == "Dhaka").FirstOrDefault());
            majorCitiesWeather.Add(db.currentWeathers.Include(c => c.City).Where(c => c.City.City_Name == "Barisal").FirstOrDefault());
            majorCitiesWeather.Add(db.currentWeathers.Include(c => c.City).Where(c => c.City.City_Name == "Khulna").FirstOrDefault());
            majorCitiesWeather.Add(db.currentWeathers.Include(c => c.City).Where(c => c.City.City_Name == "Chattogram").FirstOrDefault());
            majorCitiesWeather.Add(db.currentWeathers.Include(c => c.City).Where(c => c.City.City_Name == "Mymensingh").FirstOrDefault());
            majorCitiesWeather.Add(db.currentWeathers.Include(c => c.City).Where(c => c.City.City_Name == "Rajshahi").FirstOrDefault());
            majorCitiesWeather.Add(db.currentWeathers.Include(c => c.City).Where(c => c.City.City_Name == "Rangpur").FirstOrDefault());
            majorCitiesWeather.Add(db.currentWeathers.Include(c => c.City).Where(c => c.City.City_Name == "Sylhet").FirstOrDefault());

            Blogweather model = new Blogweather();
            model.Blogs = modelblog;
            model.CurrentWeathers = majorCitiesWeather;

            return View(model);
        }

 

        public async Task<Location> getLocation(string location)
        {
            string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + location + "&key=" + geoCoding_api_key;

            string mycontent = "";


            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (var content = response.Content)
                    {
                        mycontent = await content.ReadAsStringAsync();
                        JObject o = JObject.Parse(mycontent);
                        LocationRoot item = JsonConvert.DeserializeObject<LocationRoot>(o.ToString(), new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });

                        //string jsonString = JsonConvert.SerializeObject(item, Formatting.Indented);

                        //return jsonString;
                        //return o.ToString();
                        return item.results[0].geometry.location;
                    }
                }
            }
        }



        [HttpPost]
        public async Task<String> GetWeatherInfo(string location = "Dhaka")
        {
            var city = db.city.Where(c => c.City_Name.Equals(location)).Include(c=>c.currentWeather).FirstOrDefault();
            var dailyForeCast = db.dailyForecasts.Where(c => city.CityId == c.CityId).ToList();

            city.dailyForcasts.RemoveRange(0, city.dailyForcasts.Count() - 8);

            string jsonString = JsonConvert.SerializeObject(city, Formatting.Indented);
            return jsonString;
        }

        public async Task Update()
        {
            var city = db.city.Include(c => c.currentWeather).ToList();

            foreach (var c in city)
            {
                WeatherRoot w = await getCityWeather(c);
                CurrentWeather currentWeather = new CurrentWeather
                {
                    clouds = w.current.clouds,
                    sunrise = w.current.sunrise,
                    sunset = w.current.sunset,
                    temp = w.current.temp,
                    humidity = w.current.humidity,
                    feels_like = w.current.feels_like,
                    pressure = w.current.pressure,
                    uvi = w.current.uvi,
                    dew_point = w.current.dew_point,
                    dt = w.current.dt,
                    wind_deg = w.current.wind_deg,
                    wind_speed = w.current.wind_speed,
                    weather_main = w.current.weather[0].main,
                    weather_description = w.current.weather[0].description,
                    weather_icon = w.current.weather[0].icon,
                    weather_id = w.current.weather[0].id,
                    visibility = w.current.visibility

                };

                if (c.currentWeather == null)
                {
                    c.currentWeather = currentWeather;
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(c.currentWeather).State = EntityState.Modified;
                    c.currentWeather = currentWeather;
                    db.SaveChanges();
                }

                foreach(var dailyForecast in w.daily)
                {

                    DailyForecast dailyForecast1 = new DailyForecast
                    {
                        dt = dailyForecast.dt,
                        sunrise = dailyForecast.sunrise,
                        sunset = dailyForecast.sunset,
                        temp_day = dailyForecast.temp.day,
                        temp_min = dailyForecast.temp.min,
                        temp_max = dailyForecast.temp.max,
                        temp_night = dailyForecast.temp.night,
                        temp_eve = dailyForecast.temp.eve,
                        morn = dailyForecast.temp.morn,
                        feels_like_day = dailyForecast.feels_like.day,
                        feels_like_night = dailyForecast.feels_like.night,
                        feels_like_eve = dailyForecast.feels_like.eve,
                        feels_like_morn = dailyForecast.feels_like.morn,
                        pressure = dailyForecast.pressure,
                        humidity = dailyForecast.humidity,
                        dew_point = dailyForecast.dew_point,
                        wind_speed = dailyForecast.wind_speed,
                        wind_deg = dailyForecast.wind_deg,
                        weather_id = dailyForecast.weather[0].id,
                        weather_main = dailyForecast.weather[0].main,
                        weather_description = dailyForecast.weather[0].description,
                        weather_icon = dailyForecast.weather[0].icon,
                        clouds = dailyForecast.clouds,
                        uvi = dailyForecast.uvi
                    };

                    c.dailyForcasts.Add(dailyForecast1);
                    db.SaveChanges();
                }

            }
        }


        public async Task<WeatherRoot> getCityWeather(City city)
        {

            var url = "https://api.openweathermap.org/data/2.5/onecall?lat=" + city.Latitude + "&lon=" + city.Longitude + "&exclude=hourly,minutely&units=metric&appid=" + openWeatherMap_api_key;

            string mycontent = "";


            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (var content = response.Content)
                    {
                        mycontent = await content.ReadAsStringAsync();
                        JObject o = JObject.Parse(mycontent);
                        var item = JsonConvert.DeserializeObject<WeatherRoot>(o.ToString());
                        dynamic json = JValue.Parse(mycontent);
                        string x = json.current.temp.ToString();
                        System.Diagnostics.Debug.WriteLine(x);

                        string jsonString = JsonConvert.SerializeObject(item, Formatting.Indented);
                        return item;
                    }
                }
            }

        }
    }
}