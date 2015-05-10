using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using Shared.Entities;
using System.Diagnostics;
using Microsoft.AspNet.Identity;

namespace Chebay.BackofficeIdentity.Controllers
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

    public class DatosTipoAtributoNuevo
    {
        public string nombre { get; set; }
        public string tipoDatos { get; set; }
        public long categoria { get; set; }
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

        public string RecursionCategoriasTipoAtributo(CategoriaCompuesta categoria)
        {
            string resultado = "";
            resultado += "<li><button class=\"btn btn-link\" onclick=\"modalAgregarTipoAtributo(" + categoria.CategoriaID + ")\">" + categoria.Nombre + "</button>";
            //debo crear un arreglo JSON con las categorias
            if (categoria.hijas != null)
            {
                if (categoria.hijas.Count() > 0)
                {
                    resultado += "<ul>";
                    foreach (Categoria hija in categoria.hijas)
                    {
                        if (hija is CategoriaCompuesta)
                        {
                            resultado += RecursionCategorias((CategoriaCompuesta)hija);
                        }
                        else
                        {
                            resultado += "<li><button class=\"btn btn-link\" onclick=\"modalAgregarTipoAtributo(" + hija.CategoriaID + ")\">" + hija.Nombre + "</button></li>";
                        }
                    }
                    resultado += "</ul>";
                }


            }
            resultado += "</li>";
            return resultado;
        }

        public string RecursionCategorias(CategoriaCompuesta categoria)
        {
            string resultado = "";
            resultado += "<li><button class=\"btn btn-link\" onclick=\"modalAgregarCategoria(" + categoria.CategoriaID + ")\">" + categoria.Nombre + "</button>";
            //debo crear un arreglo JSON con las categorias
            if (categoria.hijas != null)
            {
                if (categoria.hijas.Count() > 0)
                {
                    resultado += "<ul>";
                    foreach (Categoria hija in categoria.hijas)
                    {
                        if (hija is CategoriaCompuesta)
                        {
                            resultado += RecursionCategorias((CategoriaCompuesta)hija);
                        }
                        else
                        {
                            resultado += "<li><button class=\"btn btn-link\" onclick=\"notificarCategoriaSimple()\">" + hija.Nombre + "</button></li>";
                        }
                    }
                    resultado += "</ul>";
                }
                
                
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
                string idAdmin = User.Identity.Name;
                List<AtributoSesion> atributos = idalTienda.ObtenerAtributosSesion(idAdmin);
                AtributoSesion tienda = null;
                foreach (AtributoSesion a in atributos)
                {
                    if (a.AtributoSesionID.Equals("tienda"))
                    {
                        tienda = a;
                        break;
                    }
                }
                if (datos.tipoCategoria.Equals("compuesta"))
                {
                    Categoria catCompuesta = new CategoriaCompuesta();
                    IDALTienda idal = new DALTiendaEF();
                    catCompuesta.padre = (CategoriaCompuesta)idal.ObtenerCategoria(tienda.Datos, datos.padre);
                    catCompuesta.Nombre = datos.nombre;
                    idal.AgregarCategoria(catCompuesta, tienda.Datos);
                }
                else if (datos.tipoCategoria.Equals("simple"))
                {
                    Categoria catSimple = new CategoriaSimple();
                    IDALTienda idal = new DALTiendaEF();
                    catSimple.padre = (CategoriaCompuesta)idal.ObtenerCategoria(tienda.Datos, datos.padre);
                    catSimple.Nombre = datos.nombre;
                    idal.AgregarCategoria(catSimple, tienda.Datos);
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

        //GET: /Tienda/AgregarCategoria
        [HttpPost]
        public ActionResult AgregarTipoAtributo(DatosTipoAtributoNuevo datos)
        {
            try
            {
                TipoAtributo ta = new TipoAtributo();
                ta.TipoAtributoID = datos.nombre;
                switch (datos.tipoDatos)
                {
                    case "INTEGER":
                        ta.tipodato = TipoDato.INTEGER;
                        break;
                    case "DATE":
                        ta.tipodato = TipoDato.DATE;
                        break;
                    case "BOOL":
                        ta.tipodato = TipoDato.BOOL;
                        break;
                    case "STRING":
                        ta.tipodato = TipoDato.STRING;
                        break;
                    case "FLOAT":
                        ta.tipodato = TipoDato.FLOAT;
                        break;
                    case "BINARY":
                        ta.tipodato = TipoDato.BINARY;
                        break;
                }
                string idAdmin = User.Identity.GetUserName();
                List<AtributoSesion> atributos = idalTienda.ObtenerAtributosSesion(idAdmin);
                AtributoSesion tienda = null;
                foreach (AtributoSesion a in atributos)
                {
                    if (a.AtributoSesionID.Equals("tienda"))
                    {
                        tienda = a;
                        break;
                    }
                }
                List<Categoria> categorias = new List<Categoria>();
                Categoria cat = idalTienda.ObtenerCategoria(tienda.Datos, datos.categoria);
                categorias.Add(cat);
                ta.categorias = categorias;
                idalTienda.AgregarTipoAtributo(ta, datos.categoria, tienda.Datos);
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
            string idAdmin = User.Identity.Name;
            Debug.WriteLine("Obtenercategorias::"+ idAdmin);
            List<AtributoSesion> atributos = idalTienda.ObtenerAtributosSesion(idAdmin);
            AtributoSesion tienda = null;
            foreach (AtributoSesion a in atributos)
            {
                if (a.AtributoSesionID.Equals("tienda"))
                {
                    tienda = a;
                    break;
                }
            }
            List<Categoria> categorias = idalTienda.ListarCategorias(tienda.Datos);
            tablaCategorias += "<div class=\"well\"><ul>";
            tablaCategorias += RecursionCategorias((CategoriaCompuesta) categorias.ElementAt(0));
            tablaCategorias += "</ul></div>";
            return Content(tablaCategorias);
        }

        //GET: /Tienda/ObtenerCategoriasTipoAtributo
        [HttpGet]
        public ActionResult ObtenerCategoriasTipoAtributo()
        {
            string tablaCategorias = "";
            //List<Categoria> categorias = idalTienda.ListarCategorias((string) Session["tienda"]);
            string idAdmin = User.Identity.GetUserName();
            List<AtributoSesion> atributos = idalTienda.ObtenerAtributosSesion(idAdmin);
            AtributoSesion tienda = null;
            foreach (AtributoSesion a in atributos)
            {
                if (a.AtributoSesionID.Equals("tienda"))
                {
                    tienda = a;
                    break;
                }
            }
            List<Categoria> categorias = idalTienda.ListarCategorias(tienda.Datos);
            tablaCategorias += "<div class=\"well\"><ul>";
            tablaCategorias += RecursionCategoriasTipoAtributo((CategoriaCompuesta)categorias.ElementAt(0));
            tablaCategorias += "</ul></div>";
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
                //que se hace con el admin??
                Debug.WriteLine("TiendaController::GuardarDatosGenerales::"+User.Identity.Name);
                string idAdmin = User.Identity.Name;
                List<AtributoSesion> atributos = idalTienda.ObtenerAtributosSesion(idAdmin);
                AtributoSesion tienda = null;
                foreach(AtributoSesion a in atributos){
                    if(a.AtributoSesionID.Equals("tienda")){
                        tienda = a;
                        break;
                    }
                }

                //esto no esta bien: es preferible buscar las tiendas del usuario antes de preguntar por la sesion.
                Debug.WriteLine("LUEGO FOR");
                if (tienda == null)
                {
                    AtributoSesion atr = new AtributoSesion();
                    Tienda t = new Tienda();
                    //t.TiendaID = ("/" + datosGenerales.titulo).Replace(" ", "");
                    t.TiendaID = datosGenerales.titulo;
                    t.descripcion = datosGenerales.descripcion;
                    t.nombre = datosGenerales.titulo;
                    atr.AtributoSesionID = "tienda";
                    Debug.WriteLine("ADMIN::" + idAdmin);
                    atr.Datos = t.TiendaID;
                    atr.AdministradorID = idAdmin;
                    idalTienda.AgregarAtributoSesion(atr);

                    Debug.WriteLine("MUEREAQUI!!");
                    idalTienda.AgregarTienda(t, idAdmin);
                    Debug.WriteLine("OKKK2");

                }
                else
                {
                    Tienda t = new Tienda();
                    //sacarle los espacios al string de abajo
                    //t.TiendaID = ("/" + datosGenerales.titulo).Replace(" ", "");
                    t.TiendaID = datosGenerales.titulo;
                    t.descripcion = datosGenerales.descripcion;
                    t.nombre = datosGenerales.titulo;
                    Debug.WriteLine("TiendaController::antesActualizar");
                    idalTienda.ActualizarTienda(t);
                    AtributoSesion atr = new AtributoSesion();
                    atr.AtributoSesionID = "tienda";
                    atr.Datos = t.TiendaID;
                    atr.AdministradorID = idAdmin;
                    idalTienda.AgregarAtributoSesion(atr);
                    Debug.WriteLine("OKKK");
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

    }
}
