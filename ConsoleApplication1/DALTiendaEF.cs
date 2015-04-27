using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DALTiendaEF : IDALTienda
    {
        void AgregarAdministrador(Administrador a)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    context.administradores.Add(a);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        /*void ModificarAdministrador(Administrador a)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from admin in context.administradores
                                where admin.AdministradorID == a.AdministradorID
                                select admin;
                    Administrador adm = query.FirstOrDefault();
                    adm.password = a.password;
                    adm.TiendaID = a.TiendaID;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }*/

        Administrador ObtenerAdministrador(string idAdministrador)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from admin in context.administradores
                                where admin.AdministradorID == idAdministrador
                                select admin;
                    return query.FirstOrDefault();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        Administrador ObtenerAdministradorTienda(string idTienda)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from admin in context.administradores
                                where admin.TiendaID == idTienda
                                select admin;
                    return query.FirstOrDefault();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        bool AutenticarAdministrador(string idAdministrador, string passwd)
        //Devuelve true si es el password correcto para el usuario.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from admin in context.administradores
                                where admin.AdministradorID == idAdministrador
                                select admin;
                    Administrador adm = query.FirstOrDefault();
                    if (adm != null && adm.password == passwd)
                        return true;
                    else
                        return false;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        bool CambiarPassAdministrador(string idAdministrador, string passwdVieja, string passwdNueva)
        //Devuelve true si passwdVieja es la password del idAdministrador y pudo cambiarla por passwdNueva.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from admin in context.administradores
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
                        return false;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        bool AgregarTienda(Tienda t)
        //Completa el nombre, descripción, una URL (TiendaID).
        //Devuelve FALSE si ya existe una Tienda con la misma URL.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from tienda in context.tiendas
                                where tienda.TiendaID == t.TiendaID
                                select tienda;
                    if (query != null)
                    {
                        context.tiendas.Add(t);
                        context.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        void ActualizarTienda(Tienda t)
        //Cambiar nombre o descripción de t.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from tienda in context.tiendas
                                where tienda.TiendaID == t.TiendaID
                                select tienda;
                    if (query != null)
                    {
                        Tienda tnd = query.FirstOrDefault();
                        tnd.descripcion = t.descripcion;
                        tnd.nombre = tnd.nombre;
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        bool CambiarURLTienda(string idTienda, string nuevaURL)
        //Cambia la URL de idTienda por nuevaURL.
        //Devuelve FALSE si ya existe una tienda con URL nuevaURL.
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
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        Tienda ObtenerTienda(string idTienda)
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

        List<Tienda> ObtenerTodasTiendas()
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

        Tienda ObtenerTiendasAdministrador(string idAdministrador)
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

        void AgregarAdminTienda(string idAdministrador, string idTienda)
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
        bool AgregarCategoriaCompuesta(string idCategoria, string idPadre)
        //idPadre no puede ser null. La categoría raiz se crea cuando se crea la tienda.
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
                        return false;
                    else
                    {
                        var existePadre = from p in context.categorias
                                          where p.CategoriaID == idPadre
                                          select p;
                        if (existePadre == null)
                            return false;
                        else
                        {
                            CategoriaCompuesta padre = (CategoriaCompuesta) existePadre.FirstOrDefault();
                            CategoriaCompuesta ret = new CategoriaCompuesta();
                            ret.CategoriaID = idCategoria;
                            ret.padre = padre;
                            ret.hijas = new List<Categoria>();
                            ret.atributos = new List<Atributo>();
                            padre.hijas.Add(ret);
                            context.categorias.Add(ret);
                            context.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        bool AgregarCategoriaSimple(string idCategoria, string idPadre)
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
                        return false;
                    else
                    {
                        var existePadre = from p in context.categorias
                                          where p.CategoriaID == idPadre
                                          select p;
                        if (existePadre == null)
                            return false;
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
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        //CU: 1.4 INGRESAR TIPO DE ATRIBUTO Y 1.5 ALTA ATRIBUTO
        void AgregarAtributo(string idCategoria, string idAtributo, string valor)
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

    }
}
