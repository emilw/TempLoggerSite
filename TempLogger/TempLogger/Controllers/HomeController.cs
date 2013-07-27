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
            string text = "There exists no logged temperatures";

            var repository = new TemperatureRepository(this.Request.MapPath(this.Request.ApplicationPath));

            var result = repository.Load(target).OrderByDescending(x => x.TimeStamp);

            if (result.Count() != 0)
            {
                var latestTemp = result.FirstOrDefault();
                var timeAgo = DateTime.Now.Subtract(latestTemp.TimeStamp);
                var days = timeAgo.Days;
                var hour = timeAgo.Hours;
                var minutes = timeAgo.Minutes;
                var seconds = timeAgo.Seconds;

                var output = "Temperature {0} was logged for {1} day(s), {2} hour(s), {3} minutes and {4} seconds ago";
                text = string.Format(output, latestTemp.Value, days, hour, minutes, seconds);
            }

            foreach (var item in result)
                item.TimeStamp = item.TimeStamp.ToUniversalTime();

            ViewBag.Message = text;

            return View(result);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
