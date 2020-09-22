using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeatherForecast.Models
{
    public class Blog
    {
        [Key]
        public int id { set; get; }
        public string title { set; get; }
        public string type { get; set; }
        public string imagePath { get; set; }
        public string description { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class BlogCreateViewModel
    {
        [Required]
        public string Title { set; get; }
        [Required]
        public PostType Type { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name="Related Picture")]
        public HttpPostedFileBase Picture { get; set; }
    }


    public class BlogEditViewModel
    {
        public int id { set; get; }
        [Required]
        public string Title { set; get; }


        public string Type { get; set; }
        [Required]
        public string Description { get; set; }
 
        [Display(Name = "Related Picture")]
        public HttpPostedFileBase Picture { get; set; }
    }

    public enum PostType{
        [Description("Weather Blog")]
        WeatherBlog ,
        [Description("Weather News")]
        WeatherNews
    }
}