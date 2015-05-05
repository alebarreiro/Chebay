using Shared.DataTypes;
using Shared.Entities;
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
        public void AgregarAdministrador(Administrador admin)
        {
            using (var context = ChebayDBPublic.CreatePublic())
            {
                try
                {
                    var query = from adm in context.administradores
                                where adm.AdministradorID == admin.AdministradorID
                                select adm;
                    if (query.Count() == 0)
                    {
                        context.administradores.Add(admin);
                        context.SaveChanges();
                        Debug.WriteLine("Administrador " + admin.AdministradorID + " creado correctamente");
                    }
                    else
                    {
                        throw new Exception("Ya existe un administrador " + admin.AdministradorID);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public bool AutenticarAdministrador(string idAdministrador, string passwd)
        //Devuelve true si es el password correcto para el usuario.
        {
            using (var context = ChebayDBPublic.CreatePublic())
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
                    throw;
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

        public void AgregarTienda(Tienda tienda, string idAdmin)
        //Completa el nombre, descripción, una URL (TiendaID).
        {
            using (var context = ChebayDBPublic.CreatePublic())
            {
                try
                {
               /*     if (idAdmin == "" || idAdmin == null)
                        throw new Exception("El administrador no puede ser vacío.");
                    var query = from t in context.tiendas
                                where t.TiendaID == tienda.TiendaID
                                select t;
                    if (query.Count() > 0)
                        throw new Exception("Ya existe una tienda con url " + tienda.TiendaID);
                    else
                    {
                        var ad = from a in context.administradores
                                 where a.AdministradorID == idAdmin
                                 select a;
                        if (ad.Count() == 0)
                            throw new Exception("No existe el administrador " + idAdmin);
                        else
                        {
                            if (tienda.administradores == null)
                                tienda.administradores = new HashSet<Administrador>();
                            Administrador admin = ad.FirstOrDefault();
                            tienda.administradores.Add(admin);
                            if (admin.tiendas == null)
                                admin.tiendas = new HashSet<Tienda>();
                            admin.tiendas.Add(tienda);*/
                            context.tiendas.Add(tienda);
                            ChebayDBContext.ProvisionTenant(tienda.TiendaID);
                            context.SaveChanges();
                            Debug.WriteLine("Tienda " + tienda.TiendaID + " creada con éxito.");
                            
                            //Crea la Categoria Raiz.
                            var schema = ChebayDBContext.CreateTenant(tienda.TiendaID);
                            CategoriaCompuesta raiz = new CategoriaCompuesta();
                            raiz.Nombre = "/";
                            raiz.hijas = new List<Categoria>();
                            raiz.atributos = new List<Atributo>();
                            schema.categorias.Add(raiz);
                            schema.SaveChanges();
                            Debug.WriteLine("Categoría raíz de " + tienda.TiendaID + " creada con éxito.");
                     //   }
                    //}   
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                    //Dropear las tablas creadas.
                }
            }
        }

        public void ActualizarTienda(Tienda tienda)
        //Cambiar nombre o descripción de t.
        {
            using (var context = ChebayDBPublic.CreatePublic())
            {
                try
                {
                    var qTienda = from t in context.tiendas
                                  where t.TiendaID == tienda.TiendaID
                                  select t;
                    if (qTienda.Count() > 0)
                    {
                        Tienda tnd = qTienda.FirstOrDefault();
                        tnd.descripcion = tienda.descripcion;
                        tnd.nombre = tienda.nombre;
                        context.SaveChanges();
                        Debug.WriteLine("Tienda " + tienda.TiendaID + " actualizada con éxito.");
                    }
                    else
                        throw new Exception("No existe una tienda con URL " + tienda.TiendaID);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
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

        public void AgregarCategorias(List<Categoria> lCategorias, string urlTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(urlTienda))
            {
                try
                {
                    foreach (Categoria dc in lCategorias) try
                    {
                        var query = from cat in context.categorias
                                    where cat.CategoriaID == dc.padre.CategoriaID
                                    select cat;
                        CategoriaCompuesta father = (CategoriaCompuesta)query.FirstOrDefault();
                        dc.padre = father;
                        context.categorias.Add(dc);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        throw;
                    }
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Source + e.Message);
                    throw;
                }
            }
        }

        public void AgregarCategoria(Categoria c, string urlTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(urlTienda))
            {
                try
                {
                    var query = from cat in context.categorias
                                where cat.CategoriaID == c.padre.CategoriaID
                                select cat;
                    CategoriaCompuesta father = (CategoriaCompuesta)query.FirstOrDefault();
                    c.padre = father;
                    context.categorias.Add(c);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }


        public List<Categoria> ListarCategorias(string idTienda)
        {
            using (var schema = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
                    List<Categoria> ret = new List<Categoria>();
                    var qCat = from c in schema.categorias
                               orderby c.CategoriaID
                               select c;
                    foreach (Categoria c in qCat.ToList())
                    {
                        ret.Add(c);
                    }
                    return ret;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public List<Administrador> ObtenerAdministradoresTienda(int idTienda)
        {
            return null;
        }

        public void EliminarAdministrador(string idAdministrador)
        {
            using (var context = ChebayDBPublic.CreatePublic())
            {
                try
                {
                    var query = from adm in context.administradores
                                where adm.AdministradorID == idAdministrador
                                select adm;
                    context.administradores.Remove(query.FirstOrDefault());
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public Administrador ObtenerAdministrador(string idAdministrador)
        {
            using (var context = ChebayDBPublic.CreatePublic())
            {
                try
                {
                    var query = from adm in context.administradores
                                where adm.AdministradorID == idAdministrador
                                select adm;
                    return query.FirstOrDefault();    
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public void EliminarTienda(string idTienda)
        {
            using (var context = ChebayDBPublic.CreatePublic())
            {
                try
                {
                    var query = from t in context.tiendas
                                where t.TiendaID == idTienda
                                select t;
                    Tienda tnd = query.FirstOrDefault();
                    context.tiendas.Remove(tnd);
                    context.SaveChanges();
                    Debug.WriteLine("La tienda " + idTienda + " ha sido eliminada.");
                    }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public Tienda ObtenerTienda(string idTienda)
        {
            using (var context = ChebayDBPublic.CreatePublic())
            {
                try
                {
                    var query = from t in context.tiendas
                                where t.TiendaID == idTienda
                                select t;
                    return query.FirstOrDefault();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public Categoria ObtenerCategoria(string idTienda, long idCategoria)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
                    var query = from c in context.categorias
                                    where c.CategoriaID == idCategoria
                                    select c;
                    return query.FirstOrDefault();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public void AgregarAtributo(Atributo a, string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
                    Debug.WriteLine(a.categoria.CategoriaID);
                    var query = from cat in context.categorias
                                where cat.CategoriaID == a.categoria.CategoriaID
                                select cat;
                    Categoria father = query.FirstOrDefault();
                    a.categoria = father;
                    Debug.WriteLine(a.categoria.CategoriaID);
                    context.atributos.Add(a);
                    context.SaveChanges();
                    
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public void AgregarAtributos(List<Atributo> lAtributos, string urlTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(urlTienda))
            {
                try
                {
                    foreach (Atributo a in lAtributos)
                        try
                        {

                            Debug.WriteLine(a.categoria.CategoriaID);
                            var query = from cat in context.categorias
                                        where cat.CategoriaID == a.categoria.CategoriaID
                                        select cat;
                            Categoria father = query.FirstOrDefault();
                            a.categoria = father;
                            Debug.WriteLine(a.categoria.CategoriaID);
                            context.atributos.Add(a);
                            context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                            throw;
                        }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public List<Atributo> ObtenerAtributos(long idCategoria, string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
                    List<Atributo> ret = new List<Atributo>();
                    var qCat = from cat in context.categorias
                               where cat.CategoriaID == idCategoria
                               select cat;
                    Categoria c = qCat.FirstOrDefault();
                    while (c != null)
                    {
                        foreach (Atributo a in c.atributos)
                        {
                            ret.Add(a);
                        }
                        c = c.padre;
                    }
                    return ret;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

    }
}