using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WeatherForecast.Models;

namespace WeatherForecast.Controllers
{
    public class BlogController : Controller
    {

        WeatherForecastDBContext _db;

        public BlogController()
        {
            _db = new WeatherForecastDBContext();
        }


        [HttpGet]

        public ActionResult Index(string searchDate, int? page)
        {
            if (!String.IsNullOrEmpty(searchDate))
            {
                DateTime dt = DateTime.Parse(searchDate);
                var model = _db.Blogs.Where(b => b.DateAdded.Year == dt.Year && b.DateAdded.Month == dt.Month && b.DateAdded.Day == dt.Day /*&&  b.DateAdded <=dt*/).ToList().ToPagedList(page ?? 1, 6);
                return View(model);
            }
            else
            {
                var model = _db.Blogs.ToList().ToList().ToPagedList(page ?? 1, 6);
                return View(model);
            }
        }


        public ActionResult ViewBlog(int id)
        {
            Random rand = new Random();
            int toSkip = rand.Next(1, _db.Blogs.Count());

            var blogmodel = _db.Blogs.Where(b=>b.id!=id).OrderBy(b=>b.DateAdded).Skip(toSkip).Take(4).ToList();
            var singlemodel = _db.Blogs.Where(b => b.id == id).FirstOrDefault();

            singleRelatedblog model = new singleRelatedblog();
            model.RelatedBlogs = blogmodel;
            model.singleBlog = singlemodel;

            return View(model);
        }

    }
}