﻿using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DALTiendaEF : IDALTienda
    {
        public void AgregarAdministrador(string idAdmin, string pass)
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var db = new SqlConnection(connection);
            using (var context = ChebayDBPublic.CreatePublic(db))
            {
                try
                {
                    var query = from adm in context.administradores
                                where adm.AdministradorID == idAdmin
                                select adm;
                    if (query.Count() == 0)
                    {
                        Administrador a = new Administrador();
                        a.AdministradorID = idAdmin;
                        a.password = pass;
                        context.administradores.Add(a);
                        context.SaveChanges();
                        Debug.WriteLine("Administrador " + idAdmin + " creado correctamente");
                    } 
                    else
                    {
                        throw new Exception("Ya existe un administrador " + idAdmin);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public bool AutenticarAdministrador(string idAdministrador, string passwd)
        //Devuelve true si es el password correcto para el usuario.
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var db = new SqlConnection(connection);
            using (var context = ChebayDBPublic.CreatePublic(db))
            {
                try
                {
                    var query = from admin in context.administradores
                                where admin.AdministradorID == idAdministrador
                                select admin;
                    if (query.Count() == 0)
                    {
                        throw new Exception("El nombre de usuario no es correcto.");
                    }
                    else
                    {
                        Administrador adm = query.FirstOrDefault();
                        if (adm != null && adm.password == passwd)
                        {
                            Debug.WriteLine(idAdministrador + " ha sido correctamente autenticado.");
                            return true;
                        }
                        else
                            throw new Exception("La contraseña no es correcta.");

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public void CambiarPassAdministrador(string idAdministrador, string passwdVieja, string passwdNueva)
        //Devuelve true si passwdVieja es la password del idAdministrador y pudo cambiarla por passwdNueva.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    /*var query = from admin in context.administradores
                                where admin.AdministradorID == idAdministrador
                                select admin;
                    Administrador adm = query.FirstOrDefault();
                    if (adm != null && adm.password == passwdVieja)
                    {
                        adm.password = passwdNueva;
                        context.SaveChanges();
                        return true;
                    }
                    else
                        return false;*/
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        public void AgregarTienda(string nom, string desc, string url, string idAdmin)
        //Completa el nombre, descripción, una URL (TiendaID).
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var db = new SqlConnection(connection);
            using (var context = ChebayDBPublic.CreatePublic(db))
            {
                try
                {
                    var query = from t in context.tiendas
                                where t.TiendaID == url
                                select t;
                    if (query.Count() == 0)
                    {
                        Tienda tnd = new Tienda();
                        tnd.descripcion = desc;
                        tnd.nombre = nom;
                        tnd.TiendaID = url;
                        var ad = from a in context.administradores
                                 where a.AdministradorID == idAdmin
                                 select a;
                        if (ad.Count() == 0)
                            throw new Exception("No existe el administrador " + idAdmin);
                        else
                        {
                            if (tnd.administradores == null)
                                tnd.administradores = new HashSet<Administrador>();
                            tnd.administradores.Add(ad.FirstOrDefault());
                            ad.FirstOrDefault().TiendaID = url;
                            context.tiendas.Add(tnd);
                            ChebayDBContext.ProvisionTenant(url, db);
                            context.SaveChanges();
                            Debug.WriteLine("Tienda " + url + " creada con éxito.");
                        }
                    }
                    else
                        throw new Exception("Ya existe una tienda con url " + url);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    //Dropear las tablas creadas.
                }
            }
        }

        public void ActualizarTienda(string nomNuevo, string descNueva, string urlVieja)
        //Cambiar nombre o descripción de t.
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var db = new SqlConnection(connection);
            using (var context = ChebayDBPublic.CreatePublic(db))
            {
                try
                {
                    var qTienda = from t in context.tiendas
                                where t.TiendaID == urlVieja
                                select t;
                    if (qTienda.Count() > 0)
                    {
                        Tienda tnd = qTienda.FirstOrDefault();
                        tnd.descripcion = descNueva;
                        tnd.nombre = nomNuevo;
                        context.SaveChanges();
                        Debug.WriteLine("Tienda " + urlVieja + " actualizada con éxito.");
                    }
                    else
                        throw new Exception("No existe una tienda con URL " + urlVieja);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public void CambiarURLTienda(string idTienda, string nuevaURL)
        //Cambia la URL de idTienda por nuevaURL.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from t in context.tiendas
                                where t.TiendaID == idTienda
                                select t;
                    var nueva = from n in context.tiendas
                                where n.TiendaID == nuevaURL
                                select n;

                    if (query != null && nueva == null)
                    {
                        Tienda tnd = query.FirstOrDefault();
                        tnd.TiendaID = nuevaURL;
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        public List<Tienda> ObtenerTodasTiendas()
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    return null;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public Tienda ObtenerTiendasAdministrador(string idAdministrador)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    return null;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public void AgregarAdminTienda(string idAdministrador, string idTienda)
        //Agrega a idAdministrador a idTienda.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {

                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

    //CU: 1.2 INGRESAR CATEGORIA Y 1.3 ALTA CATEGORIA
        public void AgregarCategoriaCompuesta(string idCategoria, string idPadre, string urlTienda)
        //idPadre no puede ser null. La categoría raiz se crea cuando se crea la tienda.
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var db = new SqlConnection(connection);
            
            using (var context = ChebayDBContext.CreateTenant(urlTienda, db))
            {
                try
                {
                    var existeCat = from c in context.categorias
                                    where c.CategoriaID == idCategoria
                                    select c;
                    if (existeCat.Count() > 0)
                        throw new Exception("Ya existe la categoria " + idCategoria);
                    else
                    {
                        CategoriaCompuesta ret = new CategoriaCompuesta();
                            ret.CategoriaID = idCategoria;
                            ret.hijas = new List<Categoria>();
                            ret.atributos = new List<Atributo>();
                            context.categorias.Add(ret);
                            context.SaveChanges();
                            Debug.WriteLine("Categoría " + idCategoria + " creada con éxito.");
                        }
                    }
               // }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                   
                }
            }
        }

        public void AgregarCategoriaSimple(string idCategoria, string idPadre)
        //idPadre no puede ser null.
        //Devuelve FALSE si ya existe una Categoria con el mismo nombre o no existe el padre.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var existeCat = from c in context.categorias
                                    where c.CategoriaID == idCategoria
                                    select c;
                    if (existeCat != null)
                        throw new Exception("");
                    else
                    {
                        var existePadre = from p in context.categorias
                                          where p.CategoriaID == idPadre
                                          select p;
                        if (existePadre == null)
                            throw new Exception("");
                        else
                        {
                            CategoriaCompuesta padre = (CategoriaCompuesta)existePadre.FirstOrDefault();
                            CategoriaSimple ret = new CategoriaSimple();
                            ret.CategoriaID = idCategoria;
                            ret.padre = padre;
                            ret.productos = new List<Producto>();
                            ret.atributos = new List<Atributo>();
                            padre.hijas.Add(ret);
                            context.categorias.Add(ret);
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

    //CU: Alta Subasta
        public List<Categoria> ListarCategorias(string idTienda)
        //Lista las Categorias de idTienda.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from t in context.tiendas
                                where t.TiendaID == idTienda
                                select t;
                    if (query == null)
                        return null;
                    else //Si existe la Tienda idTienda.
                    {
                        Tienda t = query.FirstOrDefault();
                        List<Categoria> ret = new List<Categoria>();
                        //Recorrer todas las categorias desde la Raiz y agregarlas a ret.
                        //En Tienda tiene que haber una referencia a la raiz.
                        return ret;
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

    //CU: 1.4 INGRESAR TIPO DE ATRIBUTO Y 1.5 ALTA ATRIBUTO
        public void AgregarAtributo(string idCategoria, string idAtributo, string valor)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {

                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        public List<Administrador> ObtenerAdministradoresTienda(int idTienda)
        {
            return null;
        }

        public void EliminarAdministrador(string idAdministrador)
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var db = new SqlConnection(connection);
            using (var context = ChebayDBPublic.CreatePublic(db))
            {
                try
                {
                    var query = from adm in context.administradores
                                where adm.AdministradorID == idAdministrador
                                select adm;
                    if (query.Count() > 0)
                    {
                        context.administradores.Remove(query.FirstOrDefault());
                        context.SaveChanges();
                        Debug.WriteLine("El administrador " + idAdministrador + " ha sido eliminado.");
                    }
                    else
                        throw new Exception("No existe un administrador " + idAdministrador);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public Administrador ObtenerAdministrador(string idAdministrador)
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var db = new SqlConnection(connection);
            using (var context = ChebayDBPublic.CreatePublic(db))
            {
                try
                {
                    var query = from adm in context.administradores
                                where adm.AdministradorID == idAdministrador
                                select adm;
                    if (query.Count() > 0)
                    {
                        return query.FirstOrDefault();
                    }
                    else
                        throw new Exception("No existe un administrador " + idAdministrador);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public void EliminarTienda(string idTienda)
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var db = new SqlConnection(connection);
            using (var context = ChebayDBPublic.CreatePublic(db))
            {
                try
                {
                    var query = from t in context.tiendas
                                where t.TiendaID == idTienda
                                select t;
                    if (query.Count() > 0)
                    {
                        Tienda tnd = query.FirstOrDefault();
                        context.tiendas.Remove(tnd);
                        context.SaveChanges();
                        Debug.WriteLine("La tienda " + idTienda + " ha sido eliminada.");
                    }
                    else
                        throw new Exception("No existe una tienda con url " + idTienda);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public Tienda ObtenerTienda(string idTienda)
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var db = new SqlConnection(connection);
            using (var context = ChebayDBPublic.CreatePublic(db))
            {
                try
                {
                    var query = from t in context.tiendas
                                where t.TiendaID == idTienda
                                select t;
                    if (query.Count() > 0)
                    {
                        return query.FirstOrDefault();
                    }
                    else
                        throw new Exception("No existe una tienda " + idTienda);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return null;
                }
            }
        }
    }
}
