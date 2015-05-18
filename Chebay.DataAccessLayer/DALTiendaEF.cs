using Shared.DataTypes;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DALTiendaEF : IDALTienda
    {
        public void AgregarAdministrador(Administrador admin)
        {
            try
            {
                if (admin == null)
                    throw new Exception("Debe pasar un administrador.");
                using (var context = ChebayDBPublic.CreatePublic())
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
                        throw new Exception("Ya existe un administrador " + admin.AdministradorID);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public bool AutenticarAdministrador(string idAdministrador, string passwd)
        //Devuelve true si es el password correcto para el usuario.
        {
            Debug.WriteLine("IDADMIN:" + idAdministrador);
            try
            {
                if (idAdministrador == null)
                    throw new Exception("Debe pasar el identificador de un administrador.");
                if (passwd == null)
                    throw new Exception("Debe pasar una contraseña.");
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    var query = from admin in context.administradores
                                where admin.AdministradorID.Equals(idAdministrador)
                                select admin;
                    if (query.Count() == 0)
                    {
                        throw new Exception("El nombre de usuario no es correcto.");
                    }
                    else
                    {
                        Administrador adm = query.FirstOrDefault();
                        if (adm != null && adm.password.Equals(passwd))
                        {
                            Debug.WriteLine(idAdministrador + " ha sido correctamente autenticado.");
                            return true;
                        }
                        else
                            throw new Exception("La contraseña no es correcta.");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void CambiarPassAdministrador(string idAdministrador, string passwdVieja, string passwdNueva)
        //Devuelve true si passwdVieja es la password del idAdministrador y pudo cambiarla por passwdNueva.
        {
            try
            {
                using (var context = new ChebayDBContext())
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void AgregarTienda(Tienda tienda, string idAdmin)
        //Completa el nombre, descripción, una URL (TiendaID).
        {
            try
            {
                if (tienda == null)
                    throw new Exception("Debe pasar una tienda.");
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    var qAdmin = from ad in context.administradores
                                 where ad.AdministradorID == idAdmin
                                 select ad;
                    if (tienda.administradores == null)
                        tienda.administradores = new HashSet<Administrador>();
                    tienda.administradores.Add(qAdmin.FirstOrDefault());
                    Personalizacion p = new Personalizacion
                    {
                        PersonalizacionID = tienda.TiendaID,
                        datos = "1"
                    };
                    context.personalizaciones.Add(p);
                    context.tiendas.Add(tienda);
                    context.SaveChanges();
                    
                    Debug.WriteLine("Creando schema...");
                    ChebayDBContext.ProvisionTenant(tienda.TiendaID);
                    Debug.WriteLine("Tienda " + tienda.TiendaID + " creada con éxito.");

                    //Crea la Categoria Raiz.
                    var schema = ChebayDBContext.CreateTenant(tienda.TiendaID);
                    CategoriaCompuesta raiz = new CategoriaCompuesta();
                    raiz.Nombre = "/";
                    raiz.hijas = new List<Categoria>();
                    schema.categorias.Add(raiz);
                    schema.SaveChanges();
                    Debug.WriteLine("Categoría raíz de " + tienda.TiendaID + " creada con éxito.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
                //Dropear las tablas creadas.
            }
        }

        public void ActualizarTienda(Tienda tienda)
        {
            try
            {
                chequearTienda(tienda.TiendaID);
                using (var context = ChebayDBPublic.CreatePublic())
                {

                    var qTienda = from t in context.tiendas
                                  where t.TiendaID == tienda.TiendaID
                                  select t;
                    Tienda tnd = qTienda.FirstOrDefault();
                    tnd.descripcion = tienda.descripcion;
                    tnd.nombre = tienda.nombre;
                    context.SaveChanges();
                    Debug.WriteLine("Tienda " + tienda.TiendaID + " actualizada con éxito.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Tienda> ObtenerTodasTiendas()
        {
            try
            {
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Tienda ObtenerTiendasAdministrador(string idAdministrador)
        {
            try
            {
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
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

        public void AgregarCategorias(List<Categoria> lCategorias, string idTienda)
        {
            try
            {
                if (lCategorias == null)
                    throw new Exception("Debe pasar una lista de categorías.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    foreach (Categoria dc in lCategorias) try
                        {
                            var query = from cat in context.categorias
                                        where cat.CategoriaID == dc.padre.CategoriaID
                                        select cat;
                            CategoriaCompuesta father = (CategoriaCompuesta)query.FirstOrDefault();
                            dc.padre = father;
                            dc.tipoatributos = new List<TipoAtributo>();
                            context.categorias.Add(dc);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                            throw e;
                        }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Source + e.Message);
                throw e;
            }
        }

        public void AgregarCategoria(Categoria c, string idTienda)
        {
            try
            {
                if (c == null)
                    throw new Exception("Debe pasar una categoría.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var query = from cat in context.categorias
                                where cat.CategoriaID == c.padre.CategoriaID
                                select cat;
                    CategoriaCompuesta father = (CategoriaCompuesta)query.FirstOrDefault();
                    c.padre = father;
                    c.tipoatributos = new List<TipoAtributo>();
                    context.categorias.Add(c);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }


        public List<Categoria> ListarCategorias(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var schema = ChebayDBContext.CreateTenant(idTienda))
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Administrador> ObtenerAdministradoresTienda(int idTienda)
        {
            return null;
        }

        public void EliminarAdministrador(string idAdministrador)
        {
            try
            {
                if (idAdministrador == null)
                    throw new Exception("Debe pasar el identificador de un administrador.");
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    var query = from adm in context.administradores
                                where adm.AdministradorID == idAdministrador
                                select adm;
                    context.administradores.Remove(query.FirstOrDefault());
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public Administrador ObtenerAdministrador(string idAdministrador)
        {
            try
            {
                if (idAdministrador == null)
                    throw new Exception("Debe pasar el identificador de un administrador.");
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    var query = from adm in context.administradores
                                where adm.AdministradorID == idAdministrador
                                select adm;
                    return query.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void EliminarTienda(string idTienda)
        {
            try
            {
                
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    var qTienda = from t in context.tiendas
                                  where t.TiendaID.Equals(idTienda)
                                  select t;
                    if (qTienda.Count() != 0)
                    {
                        var query = from t in context.tiendas
                                    where t.TiendaID == idTienda
                                    select t;
                        Tienda tnd = query.FirstOrDefault();
                        context.tiendas.Remove(tnd);
                        context.SaveChanges();
                        Debug.WriteLine("La tienda " + idTienda + " ha sido eliminada.");
                    }
                    else
                        Debug.WriteLine("La tienda " + idTienda + " no existe.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Tienda ObtenerTienda(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBPublic.CreatePublic())
                {

                    var query = from t in context.tiendas
                                where t.TiendaID == idTienda
                                select t;
                    return query.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Categoria ObtenerCategoria(string idTienda, long idCategoria)
        {
            if (idCategoria == null)
                throw new Exception("Tiene que pasar el identificador de una categoría.");
            chequearTienda(idTienda);
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var query = from c in context.categorias
                                where c.CategoriaID == idCategoria
                                select c;
                    return query.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        

        void chequearTienda(string idTienda)
        {
            try
            {
                if (idTienda == null)
                    throw new Exception("Debe pasar una tienda.");
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    
                    var qTienda = from t in context.tiendas
                                  where t.TiendaID.Equals(idTienda)
                                  select t;
                    if (qTienda.Count() == 0)
                        throw new Exception("No existe la tienda" + idTienda);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }


        public void EliminarCategoria(long idCategoria, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCategoria = from c in context.categorias
                                     where c.CategoriaID == idCategoria
                                     select c;
                    context.categorias.Remove(qCategoria.FirstOrDefault());
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void ModificarCategoria(Categoria c, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCategoria = from cat in context.categorias
                                     where cat.CategoriaID == c.CategoriaID
                                     select cat;
                    Categoria ca = qCategoria.FirstOrDefault();
                    ca.Nombre = c.Nombre;
                    ca.padre = c.padre;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

       
        public void PersonalizarTienda(string color, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    var qPers = from per in context.personalizaciones
                                where per.PersonalizacionID == idTienda
                                select per;
                    if (qPers.Count() == 0) //Si no existe personalización.
                    {
                        Personalizacion p = new Personalizacion
                        {
                            datos = color,
                            PersonalizacionID = idTienda
                        };
                        /*var qTienda = from tnd in context.tiendas
                                      where tnd.TiendaID == idTienda
                                      select tnd;
                        Tienda t = qTienda.FirstOrDefault();
                        t.personalizacion = p;*/
                        context.personalizaciones.Add(p);
                        context.SaveChanges();
                    }
                    else //Si existe y hay que actualizarla.
                    {
                        Personalizacion p = qPers.FirstOrDefault();
                        p.datos = color;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Personalizacion ObtenerPersonalizacionTienda(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    var qTienda = from tnd in context.tiendas
                                  where tnd.TiendaID == idTienda
                                  select tnd;
                    Tienda t = qTienda.FirstOrDefault();
                    return t.personalizacion;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void AgregarAtributoSesion(AtributoSesion AtributoS)
        {
            using (var context = ChebayDBPublic.CreatePublic())
            {
                //AtributoSesion atrs = context.atributossesion.Find(AtributoS.AtributoSesionID);
                var query = from a in context.administradores.Find(AtributoS.AdministradorID).atributosSesion
                            where a.AtributoSesionID.Equals(AtributoS.AtributoSesionID) 
                            
                            select a;
                
                Debug.WriteLine(AtributoS.AdministradorID + AtributoS.AtributoSesionID);

                if (query.Count() == 0)
                {
                    Debug.WriteLine("Agregando atributo...");
                    context.atributossesion.Add(AtributoS);
                    context.SaveChanges();
                }
                else
                { //update
                    Debug.WriteLine("Update atributo...");
                    query.First().Datos = AtributoS.Datos;
                    context.SaveChanges();
                }
                
            }
        }

        public void EliminarAtributoSesion(string AdminID, string AtributoID)
        {
            using (var context = ChebayDBPublic.CreatePublic())
            {
                var query = from s in context.atributossesion
                            where s.AdministradorID.Equals(AdminID) 
                            && s.AtributoSesionID.Equals(AtributoID)
                            select s;
                AtributoSesion atrs = query.FirstOrDefault(null);
                if (atrs != null)
                    context.atributossesion.Remove(atrs);
            }
        }

        public List<AtributoSesion> ObtenerAtributosSesion(string AdminID)
        {
            using (var context = ChebayDBPublic.CreatePublic())
            {
                //var query = from ats in context.atributossesion
                //            where ats.AdministradorID.Equals(AdminID)
                //            select ats;
                //return query.ToList();
                return context.administradores.Find(AdminID).atributosSesion.ToList();
            }
        }

        public void AgregarTipoAtributo(TipoAtributo ta, long idCategoria, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qTA = from tipoAtr in context.tipoatributos
                              where tipoAtr.TipoAtributoID == ta.TipoAtributoID
                              select tipoAtr;
                    TipoAtributo tipoA = qTA.FirstOrDefault();
                    if (tipoA == null)
                    {
                        var qCat = from c in context.categorias
                                   where c.CategoriaID == idCategoria
                                   select c;
                        Categoria cat = qCat.FirstOrDefault();
                        if (ta.categorias == null)
                            ta.categorias = new HashSet<Categoria>();
                        ta.categorias.Add(cat);
                        
                        if (cat.tipoatributos == null)
                            cat.tipoatributos = new HashSet<TipoAtributo>();
                        cat.tipoatributos.Add(ta);
                        context.tipoatributos.Add(ta);
                    }

                    else //update
                    {
                        Debug.WriteLine("else");
                        tipoA.tipodato = ta.tipodato;
                        /*foreach (Categoria c in ta.categorias)
                        {
                            if (c.tipoatributos == null)
                                c.tipoatributos = new HashSet<TipoAtributo>();
                            c.tipoatributos.Add(ta);
                        }*/
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void EliminarTipoAtributo(string idTipoAtributo, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qTipoA = from t in context.tipoatributos
                                 where t.TipoAtributoID == idTipoAtributo
                                 select t;
                    context.tipoatributos.Remove(qTipoA.FirstOrDefault());
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<TipoAtributo> ListarTipoAtributo(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qTipoA = from t in context.tipoatributos
                                 select t;
                    List<TipoAtributo> ret = qTipoA.ToList();
                    if (ret == null)
                        return new List<TipoAtributo>();
                    else
                        return qTipoA.ToList();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<TipoAtributo> ListarTipoAtributo(long idCategoria, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCat = from cat in context.categorias.Include("tipoatributos")
                               where cat.CategoriaID == idCategoria
                               select cat;
                    List<TipoAtributo> ret = qCat.FirstOrDefault().tipoatributos.ToList();
                    return ret;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public bool ExisteTienda(string idTienda)
        {
            using (var c = ChebayDBPublic.CreatePublic())
            {
                Tienda t = c.tiendas.Find(idTienda);
                if (t == null)
                    return false;
            }
            return true;
        }

        public List<Tienda> ListarTiendas(string idAdmin)
        {
            try
            { 
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    var qAdmin = from adm in context.administradores
                                 where adm.AdministradorID == idAdmin
                                 select adm;
                    Administrador a = qAdmin.FirstOrDefault();
                    
                    var qTiendas = from tnd in context.tiendas
                                   select tnd;
                    List<Tienda> aux = qTiendas.ToList();
                    List<Tienda> ret = new List<Tienda>();
                    foreach (Tienda t in aux)
                    {
                        if (t.administradores.Contains(a))
                            ret.Add(t);
                    }
                    return ret;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }
    }
}