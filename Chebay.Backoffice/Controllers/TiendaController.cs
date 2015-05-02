using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using Shared.Entities;

namespace Chebay.Backoffice.Controllers
{

    public class DatosGeneralesTienda
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string URL { get; set; }
    }

    public class TiendaController : Controller
    {
        
        IDALTienda idalTienda = new DALTiendaEF();

        //En cada uno de los metodos de abajo, hay que generar la vista como un string, para eso es necesario 
        //crear paginas y copiar su contenido, para una correcta generacion del codigo html

        // GET: /Tienda/CrearTienda
        [HttpGet]
        public ActionResult CrearTienda()
        {
            string pagina = "";
            string line = "";
            System.IO.StreamReader file =
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/CrearTienda.cshtml");
            while ((line = file.ReadLine()) != null)
            {
                pagina += line;
            }
            file.Close();

            return Content(pagina);
        }

        // GET: /Tienda/DatosGenerales
        [HttpGet]
        public ActionResult DatosGenerales()
        {
            string pagina = "";
            string line = "";
            System.IO.StreamReader file =
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/DatosGeneralesTienda.cshtml");
            while ((line = file.ReadLine()) != null)
            {
                pagina += line;
            }
            file.Close();
            return Content(pagina);
        }

        // GET: /Tienda/CrearCategorias
        [HttpGet]
        public ActionResult CrearCategorias()
        {
            string pagina = "";
            string line = "";
            
            System.IO.StreamReader file =
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/CrearCategorias.cshtml");
            while ((line = file.ReadLine()) != null)
            {
                pagina += line;
            }
            file.Close();
            return Content(pagina);
        }

        // GET: /Tienda/CrearTiposAtributo
        [HttpGet]
        public ActionResult CrearTiposAtributo()
        {
            string pagina = "";
            string line = "";
            System.IO.StreamReader file =
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/CrearTiposAtributo.cshtml");
            while ((line = file.ReadLine()) != null)
            {
                pagina += line;
            }
            file.Close();
            return Content(pagina);
        }

        // GET: /Tienda/CrearPersonalizacion
        [HttpGet]
        public ActionResult VisualizarCategorias()
        {
            //devolver un JSON para manipular en el javacript de CrearTienda para armar el arbol
            
            string pagina = "";
            string line = "";
            System.IO.StreamReader file =
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/CrearTiposAtributo.cshtml");
            while ((line = file.ReadLine()) != null)
            {
                pagina += line;
            }
            file.Close();
            return Content(pagina);
        }

        // GET: /Tienda/CrearPersonalizacion
        [HttpGet]
        public ActionResult CrearPersonalizacion()
        {
            string pagina = "";
            string line = "";
            System.IO.StreamReader file =
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/CrearPersonalizacion.cshtml");
            while ((line = file.ReadLine()) != null)
            {
                pagina += line;
            }
            file.Close();
            return Content(pagina);
        }

        // POST : /Tienda/FinalizarDatosGenerales
        [HttpPost]
        public ActionResult GuardarDatosGenerales(DatosGeneralesTienda datosGenerales)
        {
            //la url se generaria en logica

            try
            {
                string idAdmin = (string) Session["admin"];
                Tienda t = new Tienda();
                t.descripcion = datosGenerales.descripcion;
                t.nombre = datosGenerales.titulo;
                idalTienda.AgregarTienda(t, idAdmin);
                Session["tienda"] = t.nombre;
                var result = new { Success = "True", Message = "Se han guardado los datos generales correctamente" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = "Error al guardar los datos generales" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }

        // POST : /Tienda/FinalizarCreacionTienda
        [HttpPost]
        public ActionResult FinalizarCreacionTienda()
        {


            string pagina = "";
            string line = "";
            System.IO.StreamReader file =
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/FinalizarCreacionTienda.cshtml");
            while ((line = file.ReadLine()) != null)
            {
                pagina += line;
            }
            file.Close();
            return Content(pagina);
        }

    }
}
