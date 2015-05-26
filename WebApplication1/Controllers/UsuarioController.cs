using DataAccessLayer;
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
        private Shared.Entities.Usuario _usuarioLoggeado;

        IDALUsuario uC = new DALUsuarioEF();

        public UsuarioController()
        {
        }

        public Shared.Entities.Usuario usuarioLoggeado
        {
            get
            {
                return _usuarioLoggeado;
            }
            set
            {
                _usuarioLoggeado = value;
            }
        }

        // GET: Usuario
        [Authorize]
        public ActionResult Index()
        {
            String tienda = Session["Tienda_Nombre"].ToString();
            Debug.WriteLine(tienda);
            if (Session["Usuario"] != null)
            {
                Usuario u = (Usuario)Session["Usuario"];
                ViewBag.usuario = u;
            }
            else
            {
                String userName = User.Identity.GetUserName();
                try
                {
                    Usuario u = uC.ObtenerUsuario(userName, Session["Tienda_Nombre"].ToString());
                    Session["Usuario"] = u;
                    ImagenUsuario iu = uC.ObtenerImagenUsuario(userName, tienda);
                    ViewBag.imagenUsuario = iu.Imagen;
                    ViewBag.usuario = u;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            
            return View();
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
                    uC.EliminarImagenUsuario(usuarioId, tienda);
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
