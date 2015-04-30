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

    }
}
