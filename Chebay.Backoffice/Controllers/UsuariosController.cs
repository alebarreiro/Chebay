using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;

namespace Chebay.Backoffice.Controllers
{
    
    

    public class DatosRegistro{
        private string mail {get;set;}
        private string pass {get;set;}
    }

    public class UsuariosController : Controller
    {

        DALTiendaEF dalTienda = new DALTiendaEF();
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
            string line = "";
            //System.IO.StreamReader file =
            //    new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Usuarios/Registro.cshtml");
            //while ((line = file.ReadLine()) != null)
            //{
            //    contenido += line;
            //}
            //file.Close();
            contenido += "<h2>Registrate en Che-Bay</h2>";
            contenido += "E-mail :" + 
                            "<input type=\"text\" style=\"width : 400px;\" class=\"form-control\" id=\"mailRegistro\" placeholder=\"Ingresa tu E-mail\" /><br />";
            contenido += "Password :" +
                            "<input type=\"password\" style=\"width : 400px;\" class=\"form-control\" id=\"passRegistro\" placeholder=\"Ingresa tu Password\"><br />";
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
