﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chebay.BackofficeIdentity.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Inicio()
        {
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Bienvenido a Che-Buy.";
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
