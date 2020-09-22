using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherForecast.Models
{
    public class singleRelatedblog
    {
        public IEnumerable<Blog> RelatedBlogs { get; set; }
        public  Blog singleBlog { get; set; }
    }
}