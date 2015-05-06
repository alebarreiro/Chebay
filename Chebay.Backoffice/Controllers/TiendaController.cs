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

    public class DatosCategoriaNueva
    {
        public string nombre { get; set; }
        public long padre { get; set; }
        public string tipoCategoria { get; set; }
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
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/DatosGenerales.cshtml");
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
            //List<Categoria> categorias = idalTienda.ListarCategorias((string)Session["tienda"]);
            //Session["categorias"] = categorias;
            System.IO.StreamReader file =
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/CrearCategorias.cshtml");
            while ((line = file.ReadLine()) != null)
            {
                pagina += line;
            }
            file.Close();


            ActionResult contenido = Content(pagina);
            return contenido;
        }



        public string RecursionCategorias(CategoriaCompuesta categoria)
        {
            string resultado = "";
            resultado += "<li><button class=\"btn btn-link\" onclick=\"modalAgregarCategoria(" + categoria.CategoriaID + ")\">" + categoria.Nombre + "</button>";
            //debo crear un arreglo JSON con las categorias
            if (categoria.hijas.Count() > 0)
            {
                resultado += "<ul>";
                foreach(Categoria hija in categoria.hijas){
                    if (hija.GetType() == typeof(CategoriaCompuesta))
                    {
                        resultado += RecursionCategorias((CategoriaCompuesta) hija);
                    }
                    else
                    {
                        resultado += "<li><button class=\"btn btn-link\" onclick=\"modalAgregarCategoria(" + hija.CategoriaID + ")\">" + hija.Nombre + "</button></li>";
                    }
                }
                resultado += "</ul>";
            }
            resultado += "</li>";
            return resultado;
        }

        //GET: /Tienda/AgregarCategoria
        [HttpPost]
        public ActionResult AgregarCategoria(DatosCategoriaNueva datos)
        {
            try
            {
                if (datos.tipoCategoria.Equals("compuesta"))
                {
                    Categoria catCompuesta = new CategoriaCompuesta();
                    IDALTienda idal = new DALTiendaEF();
                    catCompuesta.padre = (CategoriaCompuesta)idal.ObtenerCategoria("Tienda Ejemplo", datos.padre);
                    catCompuesta.Nombre = datos.nombre;
                    idal.AgregarCategoria(catCompuesta, "Tienda Ejemplo");
                }
                else if (datos.tipoCategoria.Equals("simple"))
                {
                    Categoria catSimple = new CategoriaSimple();
                    IDALTienda idal = new DALTiendaEF();
                    catSimple.padre = (CategoriaCompuesta)idal.ObtenerCategoria("Tienda Ejemplo", datos.padre);
                    catSimple.Nombre = datos.nombre;
                    idal.AgregarCategoria(catSimple, "Tienda Ejemplo");
                }
                var result = new { Success = "True", Message = "Se han guardado los datos generales correctamente" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = "Error al guardar los datos generales" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //GET: /Tienda/ObtenerCategorias
        [HttpGet]
        public ActionResult ObtenerCategorias()
        {
            string tablaCategorias = "";
            //List<Categoria> categorias = idalTienda.ListarCategorias((string) Session["tienda"]);
            List<Categoria> categorias = idalTienda.ListarCategorias("Tienda Ejemplo");
            tablaCategorias += "<ul>";
            tablaCategorias += RecursionCategorias((CategoriaCompuesta) categorias.ElementAt(0));
            tablaCategorias += "</ul>";
            return Content(tablaCategorias);
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

        // POST : /Tienda/GuardarDatosGenerales
        [HttpPost]
        public ActionResult GuardarDatosGenerales(DatosGeneralesTienda datosGenerales)
        {
            //la url se generaria en logica

            try
            {
                if (Session["tienda"] == null)
                {
                    string idAdmin = (string)Session["admin"];
                    idAdmin = null;
                    Tienda t = new Tienda();
                    //t.TiendaID = ("/" + datosGenerales.titulo).Replace(" ", "");
                    t.TiendaID = datosGenerales.titulo;
                    t.descripcion = datosGenerales.descripcion;
                    t.nombre = datosGenerales.titulo;
                    idalTienda.AgregarTienda(t, idAdmin);
                    Session["tienda"] = t.nombre;
                }
                else
                {
                    string idAdmin = (string)Session["admin"];
                    idAdmin = null;
                    Tienda t = new Tienda();
                    //sacarle los espacios al string de abajo
                    //t.TiendaID = ("/" + datosGenerales.titulo).Replace(" ", "");
                    t.TiendaID = datosGenerales.titulo;
                    t.descripcion = datosGenerales.descripcion;
                    t.nombre = datosGenerales.titulo;
                    idalTienda.ActualizarTienda(t);
                    Session["tienda"] = t.nombre;
                }
                
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
