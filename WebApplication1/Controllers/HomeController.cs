using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using Shared.DataTypes;
using Shared.Entities;

namespace Frontoffice.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string urlTienda)
        {
            IDALSubasta controladorSubasta = new DALSubastaEF();
            List<DataProducto> prods = controladorSubasta.ObtenerProductosPersonalizados(urlTienda);
            if (prods == null)
            {
                prods = new List<DataProducto>();
                DataProducto p1 = new DataProducto();
                p1.nombre = "prueba 1";
                p1.descripcion = "descripcion del producto";
                DataProducto p2 = new DataProducto();
                p2.nombre = "prueba ";
                p2.descripcion = "descripcion del producto";
                prods.Add(p1);
                prods.Add(p2);
                prods.Add(p1);
                prods.Add(p2);
                prods.Add(p1);
                prods.Add(p2);
                prods.Add(p1);
                prods.Add(p2);
            }
            ViewBag.productos = prods;
            //Elegimos el estilo, por ahora los posibles valores son 1 o 2
            ViewBag.personalizacion = 1;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}