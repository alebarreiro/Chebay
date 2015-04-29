using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chebay.Backoffice.Controllers
{
    
    

    public class DatosRegistro{
        private string mail {get;set;}
        private string pass {get;set;}
    }

    public class UsuariosController : Controller
    {
        //POST: /Usuarios/Login
        [HttpPost]
        public ActionResult Login(DatosRegistro datos)
        {
            
            //invocar a la funcion de la logica de login y si no existe el usuario o los datos son erroneos
            //devolver un Json success = false
            ViewBag.LoggedIn = true;
            return Json(new { success = true, responseText = "Usted se ha loggeado correctamente." }
                , JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Usuarios/Registro
        [HttpGet]
        public ActionResult Registro()
        {
            ViewBag.LoggedIn = false;
            
            string contenido = "";
            //System.IO.StreamReader file =
            //    new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Usuarios/Registro.cshtml");
            //while ((line = file.ReadLine()) != null)
            //{
            //    contenido += line;
            //}
            //file.Close();
            contenido += "<h2>Registrate en Che-Bay</h2>";
            contenido += "" +
                            "<div class=\"input-group\"><span style=\"width: 95px;\" class=\"input-group-addon\">E-mail :</span>" +
                            "<input type=\"text\" style=\"width : 400px;\" class=\"form-control\" id=\"mailRegistro\" placeholder=\"Ingresa tu E-mail\" /></div><br />";
            contenido += "" +
                            "<div class=\"input-group\"><span style=\"width: 95px;\" class=\"input-group-addon\">Password :</span><input type=\"password\" style=\"width : 400px;\" class=\"form-control\" id=\"passRegistro\" placeholder=\"Ingresa tu Password\"></div><br />";
            contenido += "<br />" +
                        "<input type=\"button\" class=\"btn btn-success\" value=\"Confirmar\" onclick=\"ConfirmarRegistro()\"/>";
            return Content(contenido);
        }

        // GET: /Usuarios/RegistrarUsuario
        [HttpPost]
        public ActionResult RegistrarUsuario(DatosRegistro datos)
        {
            //invocar a la funcion de la logica de registrar usuario
            return Json(new { success = true, responseText = "Se ha registrado al administrador correctamente." }
                , JsonRequestBehavior.AllowGet);
        }

    }
}
