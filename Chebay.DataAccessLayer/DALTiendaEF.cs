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
                throw;
            }
        }

        public bool AutenticarAdministrador(string idAdministrador, string passwd)
        //Devuelve true si es el password correcto para el usuario.
        {
            try
            {
                if (idAdministrador == null)
                    throw new Exception("Debe pasar el identificador de un administrador.");
                if (passwd == null)
                    throw new Exception("Debe pasar una contraseña.");
                using (var context = ChebayDBPublic.CreatePublic())
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
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
                throw;
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
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
                throw;
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
                throw;
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
                throw;
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
                        context.categorias.Add(dc);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        throw;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Source + e.Message);
                throw;
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
                    context.categorias.Add(c);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
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
                throw;
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
                throw;
            }
        }

        public void EliminarTienda(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    var query = from t in context.tiendas
                                where t.TiendaID == idTienda
                                select t;
                    Tienda tnd = query.FirstOrDefault();
                    context.tiendas.Remove(tnd);
                    context.SaveChanges();
                    Debug.WriteLine("La tienda " + idTienda + " ha sido eliminada.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
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
                throw;
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
                throw;
            }
        }

        public void AgregarAtributo(Atributo a, string idTienda)
        {
            try
            {
                if (a == null)
                        throw new Exception("Tiene que pasar un atributo.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var query = from cat in context.categorias
                                where cat.CategoriaID == a.categoria.CategoriaID
                                select cat;
                    Categoria father = query.FirstOrDefault();
                    a.categoria = father;
                    context.atributos.Add(a);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public void AgregarAtributos(List<Atributo> lAtributos, string idTienda)
        {
            try
            {
                if (lAtributos == null)
                    throw new Exception("Tiene que pasar una lista de atributos.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    foreach (Atributo a in lAtributos)
                    {
                        var query = from cat in context.categorias
                                    where cat.CategoriaID == a.categoria.CategoriaID
                                    select cat;
                        Categoria father = query.FirstOrDefault();
                        a.categoria = father;
                        context.atributos.Add(a);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public List<Atributo> ObtenerAtributos(long idCategoria, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
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
                                  where t.TiendaID == idTienda
                                  select t;
                    if (qTienda.Count() == 0)
                        throw new Exception("No existe la tienda" + idTienda);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
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
                throw;
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
                throw;
            }
        }

        public Atributo ObtenerAtributo(long idAtributo, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qAtributo = from a in context.atributos
                                    where a.AtributoID == idAtributo
                                    select a;
                    return qAtributo.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public void EliminarAtributo(long idAtributo, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qAtributo = from a in context.atributos
                                    where a.AtributoID == idAtributo
                                    select a;
                    context.atributos.Remove(qAtributo.FirstOrDefault());
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public void ModificarAtributo(Atributo a, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qAtributo = from atr in context.atributos
                                    where atr.AtributoID == a.AtributoID
                                    select atr;
                    Atributo at = qAtributo.FirstOrDefault();
                    at.etiqueta = a.etiqueta;
                    at.valor = a.valor;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public void PersonalizarTienda(string color, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {

                    Debug.WriteLine(Directory.GetCurrentDirectory());
                    
                    int counter = 0;
                    string line;
                    List<string> temp = new List<string>();

                    // Read the file and display it line by line.
                    System.IO.StreamReader fileInput = new System.IO.StreamReader(@"C:\Users\Alejandro\Source\Repos\Chebay\WebApplication1\Content\personalizacion\EstiloUno\orangeStyle.css");

                    while ((line = fileInput.ReadLine()) != null)
                    {
                        temp.Add(line);
                    }
                    fileInput.Close();


                    System.IO.StreamWriter fileOutput = new System.IO.StreamWriter(@"C:\Users\Alejandro\Source\Repos\Chebay\WebApplication1\Content\personalizacion\EstiloUno\orangeStyle.css");
                    {
                        foreach (string linea in temp)
                        {
                            if (linea.Contains("    border-bottom: 20px solid #e44d26;"))
                            {
                                fileOutput.WriteLine("    border-bottom: 20px solid #63589F;");
                            }
                            else
                            {
                                fileOutput.WriteLine(linea);
                            }
                        }
                    }
                    fileOutput.Close();

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

    }
}