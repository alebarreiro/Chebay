using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chebay.Frontoffice.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string urlTienda)
        {
            if (urlTienda != null) {
                ViewBag.Message = "Bienvenido a la tienda " + urlTienda;
            }
            else
            {
                ViewBag.Message = "Bienvenido a Chebay. Ingrese la URL de la Tienda.";
            }
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
