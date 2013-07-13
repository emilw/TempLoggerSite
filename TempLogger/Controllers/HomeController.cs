using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace TempLogger.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Send(string value)
        {
            var stream  = System.IO.File.Create(this.Request.MapPath(this.Request.ApplicationPath) + "\\file.txt");

            stream.Close();
            return new EmptyResult(); 
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
