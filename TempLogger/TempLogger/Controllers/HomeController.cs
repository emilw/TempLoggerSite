using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TempLogger.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Send(string value)
        {
            var path = this.Request.MapPath(this.Request.ApplicationPath) + "\\file.txt";
            var stream = System.IO.File.Create(path);

            stream.Close();

            System.IO.File.WriteAllText(path, value);

            return new EmptyResult();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
