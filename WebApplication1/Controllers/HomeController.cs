using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using Shared.DataTypes;
using Shared.Entities;
using System.Net;
using System.Web.Security;

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
                //Si cambio de tienda destruimos la sesion
                if (Session["Tienda_Nombre"] != null && Session["Tienda_Nombre"].ToString() != urlTienda)
                {
                    FormsAuthentication.SignOut();
                    Session.Abandon(); //Destruye todos los objetos de la sesion

                    int cookieCount = Request.Cookies.Count;
                    for (var i = 0; i < cookieCount; i++)
                    {
                        var cookie = Request.Cookies[i];
                        if (cookie != null)
                        {
                            var cookieName = cookie.Name;
                            var expiredCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                            Response.Cookies.Add(expiredCookie); // overwrite it
                        }
                    }

                    // clear cookies server side
                    Request.Cookies.Clear();

                    Response.Redirect(Request.Url.ToString(), true);
                }

                List<DataProducto> prods = controladorSubasta.ObtenerProductosPersonalizados(urlTienda);
                Tienda t = controladorTienda.ObtenerTienda(urlTienda);
                //List<Categoria> categorias = controladorTienda.ListarCategorias(urlTienda);
                ViewBag.productos = prods;
                //Elegimos el estilo, por ahora los posibles valores son 1 o 2
                Personalizacion p = controladorTienda.ObtenerPersonalizacionTienda(urlTienda);
                if (p != null && (p.datos == "1" || p.datos == "2"))
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
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
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