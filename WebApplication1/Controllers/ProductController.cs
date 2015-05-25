﻿using DataAccessLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Shared.DataTypes;
using System.IO;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {

        IDALTienda cT = new DALTiendaEF();
        IDALSubasta cS = new DALSubastaEF();
        IDALUsuario cU = new DALUsuarioEF();

        public class DatosCrearProducto
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public int PrecioBase { get; set; }
            public int PrecioComprarYa { get; set; }
            public DateTime FechaCierre { get; set; }
            public long CatID { get; set; }
        }

        public class DatosCrearComentario
        {
            public string userId { get; set; }
            public DateTime fecha { get; set; }
            public string texto { get; set; }
            public long prodId { get; set; }
        }

        public class DataTipoAtributo
        {
            public string etiqueta { get; set; }
            public string tipoDato { get; set; }
            public string clave { get; set; }
        }

        public class DataAtributo
        {
            public string TipoAtributoID { get; set; }
            public string etiqueta { get; set; }
            public string valor { get; set; }
        }

        // GET: Product
        [Authorize]
        public ActionResult Index()
        {
           return View();
        }

        // GET: Product/Details/5
        public ActionResult Details(long productId)
        {
            String tienda = Session["Tienda_Nombre"].ToString();
            String userId;
            if (User.Identity.IsAuthenticated){
                userId = User.Identity.GetUserName();
            }else{
                userId = null;
            }
            Producto infoFullP = cS.ObtenerInfoProducto(productId, tienda, userId);
            ViewBag.InfoProducto = infoFullP;
            return View();
        }


        // GET: Product/CrearProducto
        [Authorize]
        public ActionResult CrearProducto()
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
            resultado += "<li><button class=\"btn btn-link disabled\" data-id=\"" + categoria.CategoriaID + "\">" + categoria.Nombre + "</button>";
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
                            //string ancestros2 = obtenerAncestros(hija.padre);
                            resultado += "<li><button class=\"btn btn-link\" id=\"item-" + hija.CategoriaID + "\" data-id=\"" + hija.CategoriaID + "\" onclick=\"seleccionarCategoriaSimple(" + hija.CategoriaID + ")\">" + hija.Nombre + "</button></li>";
                        }
                    }
                    resultado += "</ul>";
                }
            }
            resultado += "</li>";
            return resultado;
        }

        public string obtenerAncestros(CategoriaCompuesta cc)
        {
            CategoriaCompuesta actual = cc;
            List<long> ancestros = new List<long>();
            while (actual.CategoriaID != 1)
            {
                ancestros.Add(actual.CategoriaID);
                actual = actual.padre;
            }
            ancestros.Add(1);
            return string.Join(",", ancestros.ToArray());
        }

        //GET: /Product/ObtenerCategorias
        [HttpGet]
        [Authorize]
        public ActionResult ObtenerCategorias()
        {
            return PartialView("_DatosCategoriaPartial");
        }

        //GET: /Product/ObtenerComentarios
        [HttpGet]
        [Authorize]
        public ActionResult ObtenerComentarios()
        {
            return PartialView("_ComentariosPartial");
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

        /* CREAR PRODUCTO */

        // POST: Product/Create
        [HttpPost]
        public JsonResult Create(DatosCrearProducto producto, List<Atributo> atributos)
        {
            try
            {
                String idTienda = Session["Tienda_Nombre"].ToString();
                Producto p = new Producto
                {
                    nombre = producto.Titulo,
                    UsuarioID = User.Identity.GetUserName(),
                    descripcion = producto.Descripcion,
                    precio_base_subasta = producto.PrecioBase,
                    precio_compra = producto.PrecioComprarYa,
                    fecha_cierre = producto.FechaCierre,
                    CategoriaID = producto.CatID
                };

                long idProd = cS.AgregarProducto(p, idTienda);
                cS.AgregarAtributo(atributos, idProd, idTienda);

                var result = new { Success = "True", Message = idProd };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = "No se pudo crear el producto: " + e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public void UploadFile()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Obtener la imagen subida
                var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImage"];
                String usuarioId = User.Identity.Name;
                String tienda = Session["Tienda_Nombre"].ToString();
                long prodId = Convert.ToInt64(System.Web.HttpContext.Current.Request.Form["ProductoID"]);
                if (httpPostedFile != null)
                {
                    //Nombre unico
                    String fileName = Guid.NewGuid().ToString("N") + "_" + httpPostedFile.FileName;
                    // Path
                    var fileSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles"), fileName);

                    // Guardar la imagen en UplodadedFiles
                    httpPostedFile.SaveAs(fileSavePath);

                    byte[] imageBytes = System.IO.File.ReadAllBytes(fileSavePath);

                    ImagenProducto ip = new ImagenProducto
                    {
                        ProductoID = prodId,
                        Imagen = imageBytes
                    };

                    cS.AgregarImagenProducto(ip, tienda);
                }
            }
        }

        // To convert the Byte Array to the author Image
        public FileContentResult getProductImg(long productId)
        {
            String tienda = Session["Tienda_Nombre"].ToString();
            List <ImagenProducto> imgsProd = cS.ObtenerImagenProducto(productId, tienda);
            if ( imgsProd.Count > 0 ) {
                byte[] byteArray = imgsProd.First().Imagen;
                return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
            }
            else
            {
                var dir = Server.MapPath("~/Content/Images");
                var path = Path.Combine(dir, "no-photo.png");
                return new FileContentResult(System.IO.File.ReadAllBytes(path), "image/jpeg");
            }
        }

        
        // GET Producto/obtenerJsonAtributosCategoria
        [Authorize]
        [HttpGet]
        public JsonResult obtenerJsonAtributosCategoria(long catId)
        {
            String tiendaId = Session["Tienda_Nombre"].ToString();
            try
            {
                List<TipoAtributo> resultado = cT.ListarTodosTipoAtributo(catId, tiendaId);
                
                List<DataTipoAtributo> listDta = new List<DataTipoAtributo>();
                foreach (TipoAtributo ta in resultado)
                {
                    DataTipoAtributo dta = new DataTipoAtributo
                    {
                        etiqueta = ta.TipoAtributoID,
                        tipoDato = ta.tipodato.ToString(),
                        clave = ta.TipoAtributoID.Replace(" ",string.Empty)
                    };
                    listDta.Add(dta);
                }
                var result = new { Success = "True", Atributos = listDta };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        // GET Producto/obtenerDataFavorito
        [Authorize]
        [HttpGet]
        public JsonResult obtenerDataFavorito(String userId, long productId)
        {
            String tiendaId = Session["Tienda_Nombre"].ToString();
            try
            {
                int cantFavsProducto = cS.ObtenerCantFavoritos(productId, tiendaId);
                bool esFavorito = cS.EsFavorito(productId, userId, tiendaId);
                var result = new { cantFavs = cantFavsProducto, esFav = esFavorito, Success = "True"};
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //POST Producto/agregarFavorito
        [Authorize]
        [HttpPost]
        public JsonResult agregarFavorito(String userId, long productId)
        {
            String tiendaId = Session["Tienda_Nombre"].ToString();
            try
            {
                cS.AgregarFavorito(productId, userId, tiendaId);
                var result = new { Success = "True" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //POST Producto/eliminarFavorito
        [Authorize]
        [HttpPost]
        public JsonResult eliminarFavorito(String userId, long productId)
        {
            String tiendaId = Session["Tienda_Nombre"].ToString();
            try
            {
                cS.EliminarFavorito(productId, userId, tiendaId);
                var result = new { Success = "True" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //POST Producto/agregarComentario
        [Authorize]
        [HttpPost]
        public JsonResult agregarComentario(DatosCrearComentario datos)
        {
            try
            {
                String tiendaId = Session["Tienda_Nombre"].ToString();
                Comentario c = new Comentario
                {
                    texto = datos.texto,
                    fecha = datos.fecha,
                    ProductoID = datos.prodId,
                    UsuarioID = datos.userId
                };
                cS.AgregarComentario(c, tiendaId);
                var result = new { Success = "True" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //GET Producto/obtenerJsonComentarios
        [Authorize]
        [HttpGet]
        public JsonResult obtenerJsonComentarios(long prodId)
        {
            try
            {
                String tiendaId = Session["Tienda_Nombre"].ToString();
                List<Comentario> comentarios = cS.ObtenerComentarios(prodId, tiendaId);
                var result = new { Success = "True", Comentarios=comentarios };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        
        //GET Producto/obtenerJsonCalificaciones
        [HttpGet]
        public JsonResult obtenerJsonCalificaciones(string userId)
        {
            try
            {
                String tiendaId = Session["Tienda_Nombre"].ToString();
                DataCalificacion dataCal = cU.ObtenerCalificacionUsuario(userId, tiendaId);
                var result = new { Success = "True", Calificaciones = dataCal };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //GET Producto/obtenerJsonCalificaciones
        [HttpGet]
        public JsonResult obtenerJsonProductosCategoria(long catId)
        {
            try
            {
                String tiendaId = Session["Tienda_Nombre"].ToString();
                List<Producto> prods = cS.ObtenerProductosCategoria(catId, tiendaId);
                var result = new { Success = "True", Productos = prods };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //GET Producto/obtenerJsonMejoresOfertas
        [HttpGet]
        public JsonResult obtenerJsonMejoresOfertas(int N, long productId)
        {
            try
            {
                String tiendaId = Session["Tienda_Nombre"].ToString();
                List<Oferta> ofertas = cS.ObtenerOfertas(N, productId, tiendaId);
                var result = new { Success = "True", Ofertas = ofertas };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
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
