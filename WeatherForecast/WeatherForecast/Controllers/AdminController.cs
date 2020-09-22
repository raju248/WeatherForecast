using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WeatherForecast.Models;

namespace WeatherForecast.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        WeatherForecastDBContext _db;

        public AdminController()
        {
            _db = new WeatherForecastDBContext();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(AdminLoginViewModel model)
        {
            bool isValid = _db.Admins.Any(x => x.Email == model.Email && x.Password == model.Password);

            if (isValid)
            {
                Session["Email"] = model.Email;
                FormsAuthentication.SetAuthCookie(model.Email, false);
                return RedirectToAction("BlogList", "Admin");
            }

            ModelState.AddModelError("", "Invalid Username or Password!");

            return View();
        }


        public ActionResult EditProfile()
        {
            string email = User.Identity.Name;

            var user = _db.Admins.Where(a => a.Email.Equals(email)).FirstOrDefault();

            var model = new EditViewModel
            {
                id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(EditViewModel model)
        {

            if (ModelState.IsValid)
            {
                string Email = User.Identity.Name;

                if(Email.ToLower().Equals(model.Email.ToLower()))
                {
                    var admin = _db.Admins.Where(a => a.Email.ToLower().Equals(model.Email.ToLower())).FirstOrDefault();
                    admin.Name = model.Name;
                    _db.SaveChanges();

                    TempData["Success"] = "Changes Saved Successfully!";
                    return View(model);
                }

                bool isValid = _db.Admins.Any(x => x.Email == model.Email);
                if (isValid)
                {
                    ModelState.AddModelError("", "Email already exists");
                    return View(model);
                }

                var admin1 = _db.Admins.Where(a =>a.Email.Equals(Email)).FirstOrDefault();
                admin1.Email = model.Email;
                admin1.Name = model.Name;

                TempData["Success"] = "Changes Saved Successfully!";
                _db.SaveChanges();
                return RedirectToAction("Logout", "Admin");
            }

            return View(model);
        }


        public ActionResult ChangePassword()
        {
            return View();
        }


        public ActionResult AddBlog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBlog(BlogCreateViewModel model)
        {

            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(model.Picture.FileName);
                string fileExtension = Path.GetExtension(model.Picture.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                string filePathString = "/Images/Blog_Pictures/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/Blog_Pictures/"), fileName);
                model.Picture.SaveAs(fileName);

                string postType = "";

                if (model.Type == PostType.WeatherBlog)
                {
                    postType = "Weather Blog";
                }
                else
                {
                    postType = "Weather News";
                }

                var blog = new Blog
                {
                    title = model.Title,
                    type = postType,
                    description = model.Description,
                    imagePath = filePathString,
                    DateAdded = DateTime.Now
                };

                _db.Blogs.Add(blog);
                _db.SaveChanges();

                TempData["Success"] = "Successfully Added";
                ModelState.Clear();
                model = new BlogCreateViewModel();
                return View(model);
            }

            ModelState.AddModelError("", "Something went wrong, Please try again");
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(AdminRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                bool isValid = _db.Admins.Any(x => x.Email == model.Email);

                if (isValid)
                {
                    ModelState.AddModelError("", "Email already exists");
                    return View(model);
                }

                Admin admin = new Admin
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password
                };

                _db.Admins.Add(admin);
                _db.SaveChanges();
                return RedirectToAction("Login", "Admin");
            }

            return View();
        }

        [HttpGet]
        public ActionResult BlogList(string searchDate, int? page)
        {

            // string name = Session["Email"].ToString();

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

        public ActionResult Edit(int id)
        {
            var model = _db.Blogs.Where(b => b.id == id).FirstOrDefault();
            var viewModel = new BlogEditViewModel
            {
                id = model.id,
                Title = model.title,
                Description = model.description,
                Type = model.type
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(BlogEditViewModel model)
        {

            if (ModelState.IsValid)
            {
                var blog = _db.Blogs.Where(b => b.id == model.id).FirstOrDefault();

                string postType = "";

                /*if (model.Type == PostType.WeatherBlog)
                {
                    postType = "Weather Blog";
                }
                else
                {
                    postType = "Weather News";

                }*/

                if (model.Picture == null)
                {
                    blog.description = model.Description;
                    blog.title = model.Title;
                    blog.type = model.Type;

                    _db.SaveChanges();
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(model.Picture.FileName);
                    string fileExtension = Path.GetExtension(model.Picture.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                    string filePathString = "/Images/Blog_Pictures/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/Blog_Pictures/"), fileName);
                    model.Picture.SaveAs(fileName);


                    blog.imagePath = filePathString;
                    blog.description = model.Description;
                    blog.title = model.Title;
                    blog.type = postType;

                    _db.SaveChanges();
                }

                TempData["Success"] = "Successfully Added";
                ModelState.Clear();
                model = new BlogEditViewModel();
                return View(model);
            }

            return View(model);
        }


        public ActionResult Delete(int id)
        {
            var model = _db.Blogs.Where(b => b.id == id).FirstOrDefault();
            _db.Blogs.Remove(model);
            _db.SaveChanges();


            return RedirectToAction("BlogList");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}