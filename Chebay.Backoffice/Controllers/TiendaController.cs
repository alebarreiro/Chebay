using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chebay.Backoffice.Controllers
{
    public class TiendaController : Controller
    {
        //En cada uno de los metodos de abajo, hay que generar la vista como un string, para eso es necesario 
        //crear paginas y copiar su contenido, para una correcta generacion del codigo html

        // GET: /Tienda/CrearTienda

        public ActionResult CrearTienda()
        {
            return View();
        }

        // GET: /Tienda/DatosGenerales

        public ActionResult DatosGenerales()
        {
            return View();
        }

        // GET: /Tienda/CrearCategorias

        public ActionResult CrearCategorias()
        {
            return View();
        }

        // GET: /Tienda/CrearTiposAtributo

        public ActionResult CrearTiposAtributo()
        {
            return View();
        }

        // GET: /Tienda/CrearPersonalizacion

        public ActionResult CrearPersonalizacion()
        {
            return View();
        }

    }
}
