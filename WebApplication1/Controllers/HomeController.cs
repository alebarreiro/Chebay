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
        IDALTienda cT = new DALTiendaEF();
        IDALUsuario cU = new DALUsuarioEF();

        public ActionResult Index(string urlTienda)
        {
            try
            {
                //Si cambio de tienda destruimos la sesion
                if (Session["Tienda_Nombre"] != null && Session["Tienda_Anterior"] != null && Session["Tienda_Nombre"].ToString() != Session["Tienda_Anterior"].ToString())
                {
                    FormsAuthentication.SignOut();
                    Session.Abandon(); //Destruye todos los objetos de la sesion

                    //Eliminar cookies (relacionados con la tienda anterior)
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
                    // eliminar cookies server side
                    Request.Cookies.Clear();

                    Response.Redirect(Request.Url.ToString(), true);
                }
                List<DataProducto> prods = new List<DataProducto>(); 
              /*  if (User.Identity.IsAuthenticated)
                {
                    String user = User.Identity.Name;
                    DataRecomendacion dr = new DataRecomendacion{
                        UsuarioID = user,
                        productos = prods
                    };
                    DataRecomendacion drRes = cU.ObtenerRecomendacionesUsuario(urlTienda, dr);
                    ViewBag.productos = drRes.productos;
                } */
                ViewBag.productos = prods;
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