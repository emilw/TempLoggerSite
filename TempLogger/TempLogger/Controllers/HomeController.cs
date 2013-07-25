using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TempLogger.Controllers
{
    public class HomeController : Controller
    {
        TemperatureRepository _repository;
        public HomeController()
        {
            
        }

        [HttpGet]
        public ActionResult Send(string value, string target)
        {
            var repository = new TemperatureRepository(this.Request.MapPath(this.Request.ApplicationPath));

            repository.Save(new Temperature() { Value = Convert.ToDecimal(value) }, target);

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Register(string target)
        {
            var repository = new TemperatureRepository(this.Request.MapPath(this.Request.ApplicationPath));
            repository.RegisterTarget(target);
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Index(string target)
        {
            /*var path = this.Request.MapPath(this.Request.ApplicationPath) + "\\file.txt";

            string text = "No value";

            if(System.IO.File.Exists(path))
                text = System.IO.File.ReadAllText(path);*/

            string text = "No value";

            var repository = new TemperatureRepository(this.Request.MapPath(this.Request.ApplicationPath));

            var result = repository.Load(target).OrderByDescending(x => x.TimeStamp);

            if (result.Count() != 0)
            {
                var item = result.FirstOrDefault();
                text = item.Value + " - " + item.TimeStamp.ToString();
            }

            ViewBag.Message = "The latest temperature logged is " + text;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
