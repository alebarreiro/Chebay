using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using Shared.Entities;
using Shared.DataTypes;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Chebay.BackofficeIdentity.Controllers
{
    public class WebscrappingController : Controller
    {
        IDALMercadoLibreREST dalMC = new DALMercadoLibreREST();


        IDALTienda idalTienda = new DALTiendaEF();

        public class CategoriaLocal
        {
            public long idCategoria { get; set; }
        }

        public class CategoriaMC
        {
            public string categoria { get; set; }
        }

        public class CantMaxProductos
        {
            public int maxProductos { get; set; }
        }

        // POST: /Webscrapping/SeleccionarCategoriaSimple
        [HttpPost]
        public ActionResult SeleccionarCategoriaSimple(CategoriaLocal cat)
        {
            try
            {
                string idAdmin = User.Identity.Name;
                AtributoSesion atr = new AtributoSesion();
                atr.AtributoSesionID = "categoriaWebscrapping";
                atr.Datos = cat.idCategoria.ToString();
                atr.AdministradorID = idAdmin;
                idalTienda.AgregarAtributoSesion(atr);
                Debug.WriteLine("WebscrappingController::SeleccionarCategoriaSimple::cat = " + cat.idCategoria);
                string input = "<div class=\"input-group\">" +
                    "<span class=\"input-group-addon\">Cantidad de Productos :</span>" +
                    "<input type=\"text\" id=\"cantProductosMaxima\" class=\"form-control\" placeholder=\"Cantidad de máxima de productos a importar\">" +
                    "</div>";
                var result = new { Success = "True", Message = input };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }

        //POST: /Webscrapping/TraerCategorias
        [HttpPost]
        public ActionResult TraerCategorias(CantMaxProductos cantMaxProductos)
        {
            try
            {
                string resultado = "";
                string idAdmin = User.Identity.Name;
                AtributoSesion atr = new AtributoSesion();
                atr.AtributoSesionID = "cantMaxProductos";
                atr.Datos = cantMaxProductos.maxProductos.ToString();
                atr.AdministradorID = idAdmin;
                idalTienda.AgregarAtributoSesion(atr);
                Debug.WriteLine("WebscrappingController::TraerCategorias::cantMaxProductos = " + cantMaxProductos.maxProductos);
                List<DataCategoria> categoriasMC = dalMC.ListarCategoriasSitio("MLU");
                resultado += "<table class=\"table table-hover\">";
                resultado += "<thead>";
                resultado += "<tr>";
                resultado += "<th>#</th><th>Nombre de la Categoría</th>";
                resultado += "</tr>";
                resultado += "</thead>";
                resultado += "<tbody>";
                int num = 1;
                foreach(DataCategoria cat in categoriasMC){
                    resultado += "<tr onclick=\"obtenerCategoriasHijas('" + cat.id + "')\">";
                    resultado += "<td>" + num + "</td>";
                    resultado += "<td>" + cat.name + "</td>";
                    resultado += "</tr>";
                    num++;
                }
                resultado += "</tbody>";
                resultado += "</table>";
                var result = new { Success = "True", Message = resultado };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //POST : /Webscrapping/ObtenerCategoriasHijas
        [HttpPost]
        public ActionResult ObtenerCategoriasHijas(string categoria)
        {
            try
            {
                string resultado = "";
                if (categoria == null)
                {
                    Debug.WriteLine("WebscrappingController::ObtenerCategoriasHijas:: error categoria = null");
                }
                Debug.WriteLine("WebscrappingController::ObtenerCategoriasHijas:: idCategoria = " + categoria);
                List<DataCategoria> categoriasMC = dalMC.listarCategoriasHijas(categoria);
                resultado += "<table class=\"table table-hover\">";
                resultado += "<thead>";
                resultado += "<tr>";
                resultado += "<th>Nombre de la Categoría</th><th>Cantidad de Productos</th>";
                resultado += "</tr>";
                resultado += "</thead>";
                resultado += "<tbody>";
                foreach (DataCategoria cat in categoriasMC)
                {
                    resultado += "<tr onclick=\"traerProductosDeCategoria('" + cat.id + "')\">";
                    resultado += "<td>" + cat.name + "</td>";
                    resultado += "<td>" + cat.total_items_in_this_category + "</td>";
                    resultado += "</tr>";
                }
                resultado += "</tbody>";
                resultado += "</table>";
                var result = new { Success = "True", Message = resultado };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //POST : /Webscrapping/ConfirmarProductosDeCategoria
        [HttpPost]
        public ActionResult ConfirmarProductosDeCategoria(string idCategoria)
        {
            try
            {
                Debug.WriteLine("WebscrappingController::ObtenerCategoriasHijas:: categoria = " + idCategoria);
                string idAdmin = User.Identity.Name;
                List<AtributoSesion> atributos = idalTienda.ObtenerAtributosSesion(idAdmin);
                AtributoSesion cantMaxProductos = null;
                AtributoSesion tienda = null;
                AtributoSesion categoriaLocal = null;
                foreach (AtributoSesion a in atributos)
                {
                    if (a.AtributoSesionID.Equals("cantMaxProductos"))
                    {
                        cantMaxProductos = a;
                    }
                    else if (a.AtributoSesionID.Equals("tienda"))
                    {
                        tienda = a;
                    }
                    else if (a.AtributoSesionID.Equals("categoriaWebscrapping"))
                    {
                        categoriaLocal = a;
                    }
                }
                dalMC.ObtenerProductosMLporCategoria(tienda.Datos, cantMaxProductos.Datos, idCategoria.categoria, long.Parse(categoriaLocal.Datos));
                var result = new { Success = "True", Message = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            } 
        }

    }
}