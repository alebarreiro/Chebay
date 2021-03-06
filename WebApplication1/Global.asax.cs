﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Shared;
using DataAccessLayer;
using Shared.Entities;

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

                        //Inicializamos el nombre de la tienda por primera vez en la sesion
                        Session["Tienda_Desc"] = "";
                        Session["Tienda_Nombre"] = url;
                        //Personalización de la tienda
                    
                        cargarPersonalizacion(url);
                }
                else if (Session["Tienda_Nombre"].ToString() != url)
                {
                    /*Cambiamos de tienda en la misma sesion*/
                  
                    HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Cache.SetNoStore();
                    Response.Cache.SetExpires(DateTime.Now);
                    Response.Cache.SetValidUntilExpires(true);

                    //Seteamos la nueva id de la tienda
                    Debug.WriteLine(Session["Tienda_Nombre"].ToString());
                    //Debug.WriteLine(Session["Tienda_Anterior"].ToString());
                    Session["Tienda_Anterior"] = Session["Tienda_Nombre"];
                    Session["Tienda_Nombre"] = url;
                    Session["Tienda_Desc"] = "";

                    cargarPersonalizacion(url);
                }
            }
        }

        public void cargarPersonalizacion(string url)
        {
            try {
                 //Personalizacion
                IDALTienda it = new DALTiendaEF();

                String fileName = "_ViewStart.cshtml";
                var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Views"), fileName);

                Personalizacion p = it.ObtenerPersonalizacionTienda(url);
                String layout = "@{Layout = \"~/Views/Shared/_Layout.cshtml\";}";
                if (p.template != null && p.template == 1)
                {
                    layout = "@{Layout = \"~/Views/Shared/_Layout.cshtml\";}";
                    String cssFileName = "orangeStyle.css";
                    var cssFilePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/personalizacion/EstiloUno"), cssFileName);
                    if (p.css != null)
                    {
                        //Escribir css
                        System.IO.File.WriteAllText(cssFilePath, p.css);
                    }
                    else
                    {
                        var orangePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/personalizacion/EstiloUno"), "orangeStyle_backup.css");
                        String orangeStyle = System.IO.File.ReadAllText(orangePath);
                        System.IO.File.WriteAllText(cssFilePath, orangeStyle);
                    }
                }
                else if (p.template != null && p.template == 2)
                {
                    layout = "@{Layout = \"~/Views/Shared/_Layout2.cshtml\";}";

                    String cssFile = "fixes.css";
                    String cssFileBackup = "fixes_backup.css";
                    var cssPathBackup = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/personalizacion/EstiloDos"), cssFileBackup);
                    //Original css
                    String cssText = System.IO.File.ReadAllText(cssPathBackup);
                    var imgUrl = "";
                    if (p.backgroud_image != null)
                    {
                        imgUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + url + "/Product/getTiendaImg";
                    }
                    else
                    {
                        //Por default
                        imgUrl = "images/bg01.png";
                    }
                    cssText = cssText.Replace("body {}", "body {background-image: url("+ imgUrl +")}");
                    //Escribimos el css editado
                    var cssPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/personalizacion/EstiloDos"), cssFile);
                    System.IO.File.WriteAllText(cssPath, cssText);
                }
                Session["Tienda_Desc"] = p.tienda.descripcion;
                //Escribimos el layout a usar que carga todos los css para esa pers.
                System.IO.File.WriteAllText(filePath, layout);
            }
            catch (Exception ex){

            }
        }
    }
}
