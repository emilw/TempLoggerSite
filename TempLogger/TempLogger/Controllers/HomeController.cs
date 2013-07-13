﻿using System;
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

            System.IO.File.AppendAllText(path, value);

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