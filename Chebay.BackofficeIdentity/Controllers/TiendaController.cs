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

    public class DatosVerTienda
    {
        public string idTienda { get; set; }
    }

    public class DatosPersonalizacion
    {
        public string color { get; set; }
    }

    public class DatosObtenerTiposAtributo
    {
        public long idCategoria { get; set; }
    }

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

        //GET : /Tienda/VerTiendas
        [HttpGet]
        public ActionResult VerTiendas()
        {
            string pagina = "";
            pagina += "<h2>Tiendas del Administrador :</h2><br><br>";
            pagina += "<table class=\"table table-hover\">";
            pagina += "<thead>";
            pagina += "<tr>";
            pagina += "<th>";
            pagina += "Nombre";
            pagina += "</th>";
            pagina += "<th>";
            pagina += "Clickea el botón de la Tienda que quieres ver :";
            pagina += "</th>";
            pagina += "</tr>";
            pagina += "</thead>";
            pagina += "<tbody>";
            List<Tienda> tiendas = idalTienda.ListarTiendas();
            bool encontre = false;
            foreach (Tienda t in tiendas)
            {
                foreach (Administrador a in t.administradores)
                {
                    if (a.AdministradorID.Equals(User.Identity.Name))
                    {
                        encontre = true;
                        break;
                    }
                }
                if (encontre)
                {
                    pagina += "<tr>";
                    pagina += "<td>";
                    pagina += t.nombre;
                    pagina += "</td>";
                    pagina += "<td>";
                    pagina += "<button class=\"btn btn-info\" onclick=\"seleccionarTienda('" + t.TiendaID + "')\">Ver Tienda : " + t.nombre + "</button>";
                    pagina += "</td>";
                    pagina += "</tr>";
                    encontre = false;
                }
            }
            pagina += "</tbody>";
            pagina += "</table>";
            Debug.WriteLine("/Tienda/VerTiendas::contenido de pagina = " + pagina);
            return Content(pagina);
        }

        //POST: /Tienda/VerTienda
        [HttpPost]
        public ActionResult VerTienda(DatosVerTienda datos)
        {
            string pagina = "";
            string line = "";
            System.IO.StreamReader file =
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/VerTienda.cshtml");
            while ((line = file.ReadLine()) != null)
            {
                pagina += line;
            }
            file.Close();
            AtributoSesion atr = new AtributoSesion();
            atr.Datos = datos.idTienda;
            atr.AtributoSesionID = "tienda";
            idalTienda.AgregarAtributoSesion(atr);
            return Content(pagina);
        }

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
            resultado += "<li><button class=\"btn btn-link\" data-toggle=\"popover\" data-original-title=\"Agregar Tipo de Atributo\" id=\"" + categoria.CategoriaID + "\" onclick=\"mostrarPopover(" + categoria.CategoriaID + ",'" + categoria.Nombre + "')\">" + categoria.Nombre + "</button>";
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
                            resultado += RecursionCategoriasTipoAtributo((CategoriaCompuesta)hija);
                        }
                        else
                        {
                            resultado += "<li><button class=\"btn btn-link\" data-toggle=\"popover\" data-original-title=\"Agregar Tipo de Atributo\" id=\"" + hija.CategoriaID + "\" onclick=\"mostrarPopover(" + hija.CategoriaID + ",'" + hija.Nombre +"')\">" + hija.Nombre + "</button></li>";
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

        //GET: /Tienda/Personalizar
        [HttpPost]
        public ActionResult Personalizar(DatosPersonalizacion datos)
        {
            try
            {
                string idAdmin = User.Identity.Name;
                Debug.WriteLine("Obtenercategorias::" + idAdmin);
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
                idalTienda.PersonalizarTienda(datos.color, tienda.Datos);
                var result = new { Success = "True", Message = "Se ha personalizado la Tienda correctamente" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = "Error al personalizar la Tienda" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }

        //GET: /Tienda/ObtenerTiposAtributo
        [HttpGet]
        public ActionResult ObtenerTiposAtributo(DatosObtenerTiposAtributo datos)
        {
            string contenido = "";
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
            List<TipoAtributo> tipos = idalTienda.ListarTipoAtributo(datos.idCategoria, tienda.Datos);
            int cantTipos = 0;
            foreach (TipoAtributo tipo in tipos)
            {
                switch (tipo.tipodato)
                {
                    case TipoDato.INTEGER:
                        contenido += "<label class=\"label label-primary\">" + tipo.TipoAtributoID + " : " + tipo.tipodato +"</label>";
                        break;
                    case TipoDato.DATE:
                        contenido += "<label class=\"label label-danger\">" + tipo.TipoAtributoID + " : " + tipo.tipodato + "</label>";
                        break;
                    case TipoDato.STRING:
                        contenido += "<label class=\"label label-warning\">" + tipo.TipoAtributoID + " : " + tipo.tipodato + "</label>";
                        break;
                    case TipoDato.FLOAT:
                        contenido += "<label class=\"label label-info\">" + tipo.TipoAtributoID + " : " + tipo.tipodato + "</label>";
                        break;
                    case TipoDato.BOOL:
                        contenido += "<label class=\"label label-success\">" + tipo.TipoAtributoID + " : " + tipo.tipodato + "</label>";
                        break;
                    case TipoDato.BINARY:
                        contenido += "<label class=\"label label-default\">" + tipo.TipoAtributoID + " : " + tipo.tipodato + "</label>";
                        break;
                }
                if (cantTipos == 4)
                {
                    //salto de linea y reinicio contador de tipos de atributo a mostrar en el modal
                    contenido += "<br />";
                    cantTipos = 0;
                }
            }
            return Content(contenido);
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
                //ta.categorias = categorias;
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
            tablaCategorias += "<div class=\"well\" style=\"background-color : whitesmoke\"><ul>";
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
            tablaCategorias += "<div class=\"well\" style=\"background-color : whitesmoke\"><ul>";
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
            try
            {
                string idAdmin = User.Identity.Name;
                Tienda t = new Tienda();
                //sacarle los espacios al string de abajo
                //t.TiendaID = ("/" + datosGenerales.titulo).Replace(" ", "");
                t.TiendaID = datosGenerales.titulo;
                t.descripcion = datosGenerales.descripcion;
                t.nombre = datosGenerales.titulo;

                if (idalTienda.ExisteTienda(datosGenerales.titulo))
                {
                    //actualizo
                    idalTienda.ActualizarTienda(t);
                }
                else
                {
                    //agrego tienda
                    idalTienda.AgregarTienda(t, idAdmin);
                }
                //cambio atributo sesion para categorias
                AtributoSesion atr = new AtributoSesion();
                atr.AtributoSesionID = "tienda";
                atr.Datos = t.TiendaID;
                atr.AdministradorID = idAdmin;

                Debug.WriteLine("TiendaController::GuardarDatosgenerales::GuardarAtributo");
                idalTienda.AgregarAtributoSesion(atr);
                    
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
