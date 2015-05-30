using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Frontoffice
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            if (System.Web.HttpContext.Current.Session != null &&
                Request.Url.Segments.Count() > 1)
            {
                String url = "";
                if (Request.Url.Segments[1].EndsWith("/"))
                {
                    url = Request.Url.Segments[1].Substring(0, Request.Url.Segments[1].Length - 1);
                }
                else
                {
                    url = Request.Url.Segments[1];
                }
                if (Session["Tienda_Nombre"] == null)
                {
                    /* Inciamos la tienda por primera vez en la sesion */
                    //Personalización de la tienda
                    String fileName = "_ViewStart.cshtml";
                    var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Views"), fileName);
                    String text = "@{Layout = \"~/Views/Shared/_Layout2.cshtml\";}";
                    System.IO.File.WriteAllText(filePath, text);
                    //Inicializamos el nombre de la tienda por primera vez en la sesion
                    Session["Tienda_Nombre"] = url;
                }
                else if (Session["Tienda_Nombre"].ToString() != url)
                {
                    /*Cambiamos de tienda en la misma sesion*/
                    //Personalización de la tienda
                    String fileName = "_ViewStart.cshtml";
                    var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Views"), fileName);
                    String text = "@{Layout = \"~/Views/Shared/_Layout.cshtml\";}";
                    System.IO.File.WriteAllText(filePath, text);
                    //Seteamos la nueva id de la tienda
                    Session["Tienda_Anterior"] = Session["Tienda_Nombre"];
                    Session["Tienda_Nombre"] = url;
                }
            }
        }
    }
}
