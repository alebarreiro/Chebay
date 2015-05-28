﻿using DataAccessLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.IO;

namespace WebApplication1.Controllers
{
    public class UsuarioController : Controller
    {

        IDALUsuario uC = new DALUsuarioEF();

        public class DatosUsuario
        {
            public string UsuarioID { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Pais { get; set; }
            public string Ciudad { get; set; }
            public string Direccion { get; set; }
            public string NumeroContacto { get; set; }
        }

        public class DataProductoUsuario
        {
            public long ProductoID { get; set; }
            public string nombre { get; set; }
        }

        public class DataCompraUsuario
        {
            public long ProductoID { get; set; }
            public int monto { get; set; }
        }

        // GET: Usuario
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult obtenerJsonDatosUsuario(string userId)
        {
            try
            {
                Usuario u = uC.ObtenerUsuario(userId, Session["Tienda_Nombre"].ToString());
                var result = new { Success = "True", Usuario = u };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }

        // To convert the Byte Array to the author Image
        public FileContentResult getUserImg(String userId)
        {
            String tienda = Session["Tienda_Nombre"].ToString();
            ImagenUsuario iu = uC.ObtenerImagenUsuario(userId, tienda);
            if (iu != null)
            {
                byte[] byteArray = iu.Imagen; 
                return byteArray != null
                 ? new FileContentResult(byteArray, "image/jpeg")
                 : null;
            }
            else
            {
                var dir = Server.MapPath("~/Content/Images");
                var path = Path.Combine(dir, "user_sin_imagen.png");
                return new FileContentResult(System.IO.File.ReadAllBytes(path), "image/jpeg");
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult actualizarDatosUsuario(DatosUsuario du)
        {
            try
            {
                Usuario u = new Usuario
                {
                    UsuarioID = du.UsuarioID,
                    Nombre = du.Nombre,
                    Apellido = du.Apellido,
                    Pais = du.Pais,
                    Ciudad = du.Ciudad,
                    Direccion = du.Direccion,
                    NumeroContacto = du.NumeroContacto
                };
                uC.ActualizarUsuario(u, Session["Tienda_Nombre"].ToString());
                var result = new { Success = "True", Message = "Usuario actualizado" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.Message };
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
                if (httpPostedFile != null)
                {
                    //Nombre unico
                    String fileName = Guid.NewGuid().ToString("N") + "_" + httpPostedFile.FileName;
                    // Path
                    var fileSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles"), fileName);

                    // Guardar la imagen en UplodadedFiles
                    httpPostedFile.SaveAs(fileSavePath);

                    byte[] imageBytes = System.IO.File.ReadAllBytes(fileSavePath);

                    ImagenUsuario iu = new ImagenUsuario{
                        UsuarioID = usuarioId,
                        Imagen    = imageBytes
                    };
                    //uC.EliminarImagenUsuario(usuarioId, tienda);
                    uC.AgregarImagenUsuario(iu, tienda);
                }
            }
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuario/Edit/5
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

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuario/Delete/5
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
