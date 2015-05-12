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
        IDALSubasta controladorSubasta = new DALSubastaEF();
        IDALTienda controladorTienda = new DALTiendaEF();

        public ActionResult Index(string urlTienda)
        {
            try
            {
                List<DataProducto> prods = controladorSubasta.ObtenerProductosPersonalizados(urlTienda);
                Tienda t = controladorTienda.ObtenerTienda(urlTienda);
                //List<Categoria> categorias = controladorTienda.ListarCategorias(urlTienda);
                ViewBag.productos = prods;
                //Elegimos el estilo, por ahora los posibles valores son 1 o 2
                Personalizacion p = controladorTienda.ObtenerPersonalizacionTienda(urlTienda);
                if (p.datos == "1" || p.datos == "2")
                {
                    Session["Tienda_Personalizacion"] = p.datos;
                }
                else
                {
                    Session["Tienda_Personalizacion"] = "1";
                }
                Session["Tienda_Nombre"] = urlTienda;
                ViewBag.Message = urlTienda;
            }
            catch (Exception e)
            {
                
            }
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