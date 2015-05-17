using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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
                Request.Url.Segments.Count() > 1 && 
                Session["Tienda_Nombre"] == null)
            {
                //Para inicializar el nombre de la tienda
                Session["Tienda_Nombre"] = Request.Url.Segments[1];
            } 
            else if (System.Web.HttpContext.Current.Session != null && 
                Request.Url.Segments.Count() > 1 && 
                Session["Tienda_Nombre"] != null &&
                Session["Tienda_Nombre"].ToString() != Request.Url.Segments[1])
            {
                //Si cambiamos la tienda en la misma sesion
                Session["Tienda_Nombre"] = Request.Url.Segments[1];
            }
        }
    }


}
