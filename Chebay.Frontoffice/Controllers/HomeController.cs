using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using Shared.DataTypes;

namespace Chebay.Frontoffice.Controllers
{
    public class HomeController : Controller
    {
        IDALSubasta controladorSubasta = new DALSubastaEF();

        public ActionResult Index(string urlTienda)
        {
            if (urlTienda != null) {
                ViewBag.Message = urlTienda;
                
            }
            else
            {
                ViewBag.Message = "Ingresa /{nombreTienda}";
            }
            List<DataProducto> prods = controladorSubasta.ObtenerProductosPersonalizados("Tienda1");
            ViewBag.productos = prods;
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
