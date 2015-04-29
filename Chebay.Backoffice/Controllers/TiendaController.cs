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
            string pagina = "<div class=\"btn-group\">"
                + "<button type=\"button\" class=\"btn btn-primary\" onclick=\"datosGenerales()\">Datos Generales&nbsp;&nbsp;<span class=\"badge pull-right\">1</span></button>"
                + "<button type=\"button\" class=\"btn btn-success\" onclick=\"crearCategorias()\">Categorías&nbsp;&nbsp;<span class=\"badge pull-right\">2</span></button>"
                + "<button type=\"button\" class=\"btn btn-warning\" onclick=\"crearTiposAtributo()\">Tipos de Atributo&nbsp;&nbsp;<span class=\"badge pull-right\">3</span></button>"
                + "<button type=\"button\" class=\"btn btn-danger\" onclick=\"crearPersonalizacion()\">Personalización&nbsp;&nbsp;<span class=\"badge pull-right\">4</span></button>"
                + "</div>"
                + "<div id=\"contenidoCrearTienda\" class=\"contenidoCrearTienda\">"
                + "</div>";
            return Content(pagina);
        }

        // GET: /Tienda/DatosGenerales

        public ActionResult DatosGenerales()
        {
            string pagina = "<br><br>"
                            + "<div class=\"input-group\">"
                            + "<span class=\"input-group-addon\">Título :</span>"
                            + "<input type=\"text\" id=\"tituloTienda\" class=\"form-control\" placeholder=\"Título de la Tienda\">"
                            + "</div>"
                            + "<br><br>"
                            + "Descripción :<br>"
                            + "<textarea id=\"descripcionTienda\" class=\"form-control\" placeholder=\"Descripción de la Tienda\"></textarea>";
            return Content(pagina);
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
