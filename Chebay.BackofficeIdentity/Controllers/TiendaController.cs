using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using Shared.Entities;
using Shared.DataTypes;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Chebay.BackofficeIdentity.Controllers
{

    public class DatosVerTienda
    {
        public string tienda { get; set; }
    }

    public class PersonalizacionEstiloUno
    {
        public string colorPrimario { get; set; }
        public string colorSecundario { get; set; }
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
        public string tipo { get; set; }
        public long categoria { get; set; }
    }

    public class TiendaController : Controller
    {
        
        IDALTienda idalTienda = new DALTiendaEF();

        //En cada uno de los metodos de abajo, hay que generar la vista como un string, para eso es necesario 
        //crear paginas y copiar su contenido, para una correcta generacion del codigo html

        //GET : /Tienda/VerReporteTienda
        [HttpGet]
        public ActionResult VerReporteTienda(DatosVerTienda datos)
        {
            try
            {   

                DataReporte reporte = idalTienda.ObtenerReporte(datos.tienda);
                string resultado = "<table class=\"table table-hover\">";
                resultado += "<tr class=\"active\" style=\"font-weight : bold\">";
                resultado += "<td>Usuarios : </td>";
                resultado += "<td>" + reporte.cantUsuarios + "</td>";
                resultado += "</tr>";
                resultado += "<tr class=\"primary\" style=\"font-weight : bold\">";
                resultado += "<td>Transacciones : </td>";
                resultado += "<td>" + reporte.cantTransacciones + "</td>";
                resultado += "</tr>";
                resultado += "</table>";

                var result = new { Success = "True", Message = resultado };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = "Error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        //GET : /Tienda/ObtenerPaginaTiendas
        [HttpGet]
        public ActionResult ObtenerPaginaTiendas(int paginaTienda)
        {
            string pagina = "";
            pagina += "<h2>Tiendas del Administrador :</h2><br><br>";
            string paginador = "<ul class=\"pagination\">";
            int cantPaginas = idalTienda.ObtenerCantPaginas(User.Identity.Name);
            for (int i = 1; i <= cantPaginas; i++)
            {
                paginador += "<li><a onclick=\"obtenerPaginaTiendas(" + i + ")\">" + i + "</a></li>";
            }
            paginador += "</ul>";
            pagina += paginador;
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
            List<Tienda> tiendas = idalTienda.ObtenerPagina(paginaTienda, User.Identity.Name);
            foreach (Tienda t in tiendas)
            {
                pagina += "<tr>";
                pagina += "<td>";
                pagina += t.nombre;
                pagina += "</td>";
                pagina += "<td>";
                pagina += "<button class=\"btn btn-info\" onclick=\"seleccionarTienda('" + t.TiendaID + "')\">Ver Tienda</button>";
                pagina += "</td>";
                pagina += "<td>";
                pagina += "<button class=\"btn btn-danger\" onclick=\"modalBorrarTienda('" + t.TiendaID + "')\">Borrar Tienda</button>";
                pagina += "</td>";
                pagina += "<td>";
                pagina += "<button class=\"btn btn-success\" onclick=\"modalReporteTienda('" + t.TiendaID + "')\">Ver Reporte</button>";
                pagina += "</td>";
                pagina += "</tr>";
            }
            pagina += "</tbody>";
            pagina += "</table>";
            Debug.WriteLine("/Tienda/VerTiendas::contenido de pagina = " + pagina);
            return Content(pagina);
        }


        //GET : /Tienda/VerTiendas
        [HttpGet]
        public ActionResult VerTiendas()
        {
            string pagina = "";
            pagina += "<h2>Tiendas del Administrador :</h2><br><br>";
            string paginador = "<ul class=\"pagination\">";
            int cantPaginas = idalTienda.ObtenerCantPaginas(User.Identity.Name);
            for(int i = 1; i <= cantPaginas; i++){
                paginador += "<li><a onclick=\"obtenerPaginaTiendas(" + i + ")\">" + i + "</a></li>";
            }
            paginador += "</ul>";
            pagina += paginador;
            pagina += "<table class=\"table table-hover\">";
            pagina += "<thead>";
            pagina += "<tr>";
            pagina += "<th>";
            pagina += "Nombre";
            pagina += "</th>";
            pagina += "<th>";
            pagina += "Ver Tienda :";
            pagina += "</th>";
            pagina += "<th>";
            pagina += "Borrar Tienda :";
            pagina += "</th>";
            pagina += "</tr>";
            pagina += "</thead>";
            pagina += "<tbody>";
            List<Tienda> tiendas = idalTienda.ObtenerPagina(1, User.Identity.Name);
            //List<Tienda> tiendas = idalTienda.ListarTiendas(User.Identity.Name);
            foreach (Tienda t in tiendas)
            {
                    pagina += "<tr>";
                    pagina += "<td>";
                    pagina += t.nombre;
                    pagina += "</td>";
                    pagina += "<td>";
                    pagina += "<button class=\"btn btn-info\" onclick=\"seleccionarTienda('" + t.TiendaID + "')\">Ver Tienda</button>";
                    pagina += "</td>";
                    pagina += "<td>";
                    pagina += "<button class=\"btn btn-danger\" onclick=\"modalBorrarTienda('" + t.TiendaID + "')\">Borrar Tienda</button>";
                    pagina += "</td>";
                    pagina += "<td>";
                    pagina += "<button class=\"btn btn-success\" onclick=\"modalReporteTienda('" + t.TiendaID + "')\">Ver Reporte</button>";
                    pagina += "</td>";
                    pagina += "</tr>";
            }
            pagina += "</tbody>";
            pagina += "</table>";
            Debug.WriteLine("/Tienda/VerTiendas::contenido de pagina = " + pagina);
            return Content(pagina);
        }

        //POST: /Tienda/BorrarTienda
        [HttpGet]
        public ActionResult BorrarTienda(string tienda)
        {
            try
            {
                idalTienda.EliminarTienda(tienda);
                var result = new { Success = "True", Message = "Success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                var result = new { Success = "False", Message = "Error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
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
            atr.Datos = datos.tienda;
            atr.AdministradorID = User.Identity.Name;
            atr.AtributoSesionID = "tienda";
            idalTienda.AgregarAtributoSesion(atr);
            Debug.WriteLine("La tienda seleccionada es : " + datos.tienda);
            var result = new { Success = "True", Message = pagina };
            return Json(result, JsonRequestBehavior.AllowGet);
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
            resultado += "<li><button class=\"btn btn-link\"  style=\"font-weight : bold; font-size : 18px; font-family: monospace;\"  data-toggle=\"popover\" data-original-title=\"Agregar Tipo de Atributo\" id=\"" + categoria.CategoriaID + "\" onclick=\"mostrarPopover(" + categoria.CategoriaID + ",'" + categoria.Nombre + "')\">" + categoria.Nombre + "</button>";
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
                            resultado += "<li><button class=\"btn btn-link\" data-toggle=\"popover\" data-original-title=\"Agregar Tipo de Atributo\" id=\"" + hija.CategoriaID + "\" onclick=\"mostrarPopover(" + hija.CategoriaID + ",'" + hija.Nombre + "')\">" + hija.Nombre + "</button></li>";
                        }
                    }
                    resultado += "</ul><hr>";
                }


            }
            resultado += "</li>";
            return resultado;
        }

        public string RecursionCategorias(CategoriaCompuesta categoria)
        {
            string resultado = "";
            resultado += "<li><button class=\"btn btn-link\" style=\"font-weight : bold; font-size : 18px; font-family: monospace;\"  onclick=\"modalAgregarCategoria(" + categoria.CategoriaID + ")\">" + categoria.Nombre + "</button>";
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
                            resultado += "<li><button class=\"btn btn-link\" onclick=\"modalSeleccionarCategoriaWebScrapping(" + hija.CategoriaID + ")\">" + hija.Nombre + "</button></li>";
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

        //GET: /Tienda/PersonalizarEstiloUno
        [HttpPost]
        public ActionResult PersonalizarEstiloUno(PersonalizacionEstiloUno datos)
        {
            try
            {
                string idAdmin = User.Identity.Name;
                Debug.WriteLine("Personalizar::" + idAdmin);
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
                Debug.WriteLine("Personalizar::valor del color = " + datos.colorPrimario);
                
                idalTienda.PersonalizarTienda(datos.colorPrimario, datos.colorSecundario, 1, null, tienda.Datos);
                var result = new { Success = "True", Message = "Se ha personalizado la Tienda correctamente" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = "Error al personalizar la Tienda" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }

        //POST : /Tienda/PersonalizarEstiloDos
        [HttpPost]
        public ActionResult PersonalizarEstiloDos()
        {
            try
            {
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
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        Debug.WriteLine("El nombre del archivo subido es : " + fileContent.FileName);
                        Personalizacion p = idalTienda.ObtenerPersonalizacionTienda(tienda.Datos);
                        if (p == null)
                        {
                            p = new Personalizacion();
                        }
                        byte[] buf;
                        buf = new byte[stream.Length];  //declare arraysize
                        stream.Read(buf, 0, buf.Length);
                        //aca iria lo de personalizar la tienda 
                        idalTienda.PersonalizarTienda(null, null, 2, buf, tienda.Datos);

                        //var fileName = Path.GetFileName(file);
                        //var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                        //using (var fileStream = File.Create(path))
                        //{
                        //  stream.CopyTo(fileStream);
                        //}
                    }
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    Success = false
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Success = true
            }, JsonRequestBehavior.AllowGet);
        }

        //POST: /Tienda/ObtenerTiposAtributo
        [HttpPost]
        public ActionResult ObtenerTiposAtributo(DatosObtenerTiposAtributo datos)
        {
            string contenido = "";
            string idAdmin = User.Identity.GetUserName();
            List<AtributoSesion> atributos = idalTienda.ObtenerAtributosSesion(idAdmin);
            AtributoSesion tienda = null;
            Debug.WriteLine("ObtenerTiposAtributo::idCategoria = " + datos.idCategoria);
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
            Debug.WriteLine("ObtenerTiposAtributo::cantidad de tipos de atributo de la categoria = " + tipos.Count);
            foreach (TipoAtributo tipo in tipos)
            {
                Debug.WriteLine("ObtenerTiposAtributo::tipo de datos del tipo de atributo " + tipo.TipoAtributoID + " = " + tipo.tipodato);
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
                cantTipos++;
                if (cantTipos == 5)
                {
                    //salto de linea y reinicio contador de tipos de atributo a mostrar en el modal
                    contenido += "<br /><br />";
                    cantTipos = 0;
                }
                else
                {
                    contenido += "&nbsp;&nbsp;&nbsp";
                }
            }
            if (cantTipos == 0)
            {
                contenido += "La categoría seleccionada no tiene tipos de atributo";
            }
            Debug.WriteLine("ObtenerTiposAtributo::contenido = " + contenido);
            var result = new { Success = "True", Message = contenido };
            return Json(result, JsonRequestBehavior.AllowGet);
            //return Content(contenido);
        }


        //GET: /Tienda/AgregarCategoria
        [HttpPost]
        public ActionResult AgregarTipoAtributo(DatosTipoAtributoNuevo datos)
        {
            try
            {
                TipoAtributo ta = new TipoAtributo();
                ta.TipoAtributoID = datos.nombre;
                Debug.WriteLine("AgregarTipoAtributo::tipo de datos del tipo de atributo a agregar = " + datos.tipo);
                switch (datos.tipo)
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

        //POST: /Tienda/SubirAlgoritmoRecomendacion
        [HttpPost]
        public ActionResult SubirAlgoritmoRecomendacion()
        {
            try
            {
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
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        Debug.WriteLine("El nombre del archivo subido es : " + fileContent.FileName);
                        Personalizacion p = idalTienda.ObtenerPersonalizacionTienda(tienda.Datos);
                        byte[] buf;
                        buf = new byte[stream.Length];  //declare arraysize
                        stream.Read(buf, 0, buf.Length);
                        p.algoritmo = buf;
                        idalTienda.ActualizarAlgoritmoPersonalizacion(p);
                        //var fileName = Path.GetFileName(file);
                        //var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                        //using (var fileStream = File.Create(path))
                        //{
                          //  stream.CopyTo(fileStream);
                        //}
                    }
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    Success = false
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Success = true
            }, JsonRequestBehavior.AllowGet);
            
        }

        // GET: /Tienda/AlgoritmoRecomendacion
        [HttpGet]
        public ActionResult AlgoritmoRecomendacion()
        {
            string pagina = "";
            string line = "";
            System.IO.StreamReader file =
                new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/Views/Tienda/AlgoritmoRecomendacion.cshtml");
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
                t.TiendaID = (datosGenerales.titulo).Replace(" ", "");
                //t.TiendaID = datosGenerales.titulo;
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
