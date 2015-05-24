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
        public ActionResult Index(String tienda)
        {
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
                    ViewBag.usuario = u;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            
            return View();
        }

        [HttpPost]
        public void UploadFile()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Obtener la imagen subida
                var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImage"];

                if (httpPostedFile != null)
                {
                    // Path
                    var fileSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);

                    // Guardar la imagen en UplodadedFiles
                    httpPostedFile.SaveAs(fileSavePath);
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
