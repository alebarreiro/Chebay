using DataAccessLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {

        IDALTienda cT = new DALTiendaEF();
        IDALSubasta cS = new DALSubastaEF();

        public class DatosCrearProducto
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public int PrecioBase { get; set; }
            public int PrecioComprarYa { get; set; }
            public DateTime FechaCierre { get; set; }
            public long CatID { get; set; }
        }

        // GET: Product
        [Authorize]
        public ActionResult Index()
        {
           return View();
        }

        public ActionResult DatosProducto()
        {
            return PartialView("_DatosProductoPartial");
        }

        public string RecursionCategorias(CategoriaCompuesta categoria)
        {
            string resultado = "";
            resultado += "<li><button class=\"btn btn-link\">" + categoria.Nombre + "</button>";
            //debo crear un arreglo JSON con las categorias
            if (categoria.hijas != null)
            {
                if (categoria.hijas.Count() > 0)
                {
                    resultado += "<ul>";
                    foreach (Categoria hija in categoria.hijas)
                    {
                        if (hija is CategoriaCompuesta)
                        {
                            resultado += RecursionCategorias((CategoriaCompuesta)hija);
                        }
                        else
                        {
                            resultado += "<li><button class=\"btn btn-link\" data-id=\"" + hija.CategoriaID + "\"  onclick=\"seleccionarCategoriaSimple("+hija.CategoriaID+")\">" + hija.Nombre + "</button></li>";
                        }
                    }
                    resultado += "</ul>";
                }


            }
            resultado += "</li>";
            return resultado;
        }

        //GET: /Product/ObtenerCategorias
        [HttpGet]
        [Authorize]
        public ActionResult ObtenerCategorias()
        {
            return PartialView("_DatosCategoriaPartial");
        }

        public ContentResult MostrarCategorias()
        {
            string tablaCategorias = "";
            List<Categoria> categorias = cT.ListarCategorias(Session["Tienda_Nombre"].ToString());
            tablaCategorias += "<div style=\"background-color : white;\"><ul>";
            tablaCategorias += RecursionCategorias((CategoriaCompuesta)categorias.ElementAt(0));
            tablaCategorias += "</ul></div>";
            return Content(tablaCategorias);
        }



        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public JsonResult Create(DatosCrearProducto producto)
        {
            try
            {
                Producto p = new Producto
                {
                    nombre = producto.Titulo,
                    UsuarioID = User.Identity.GetUserName(),
                    descripcion = producto.Descripcion,
                    precio_base_subasta = producto.PrecioBase,
                    precio_compra = producto.PrecioComprarYa,
                    fecha_cierre = producto.FechaCierre
                };


                CategoriaSimple cs = (CategoriaSimple)cT.ObtenerCategoria(Session["Tienda_Nombre"].ToString(), producto.CatID);
                if (p.categorias == null)
                    p.categorias = new HashSet<CategoriaSimple>();
                p.categorias.Add(cs);
                cS.AgregarProducto(p, Session["Tienda_Nombre"].ToString());
                Debug.WriteLine(producto.Titulo);
                var result = new { Success = "True", Message = producto };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = "No se pudo crear el producto :("};
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
