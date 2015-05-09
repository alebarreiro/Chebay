using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;

namespace DataAccessLayer
{
    public class DALUsuarioEF : IDALUsuario
    {
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
                throw e;
            }
        }

        public void AgregarUsuario(Usuario u, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                
                    context.usuarios.Add(u);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void ActualizarUsuario(Usuario u, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var query = from usr in context.usuarios
                                where usr.UsuarioID == u .UsuarioID
                                select usr;

                    Usuario user = query.FirstOrDefault();

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Usuario ObtenerUsuario(string idUsuario, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var query = from usr in context.usuarios
                                where usr.UsuarioID == idUsuario
                                select usr;
                    return query.First();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Usuario> ObtenerTodosUsuarios(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var query = from usr in context.usuarios
                                select usr;    
                    return query.ToList();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void EliminarUsuario(string idUsuario, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var query = from usr in context.usuarios
                                where usr.UsuarioID == idUsuario
                                select usr;
                    context.usuarios.Remove(query.FirstOrDefault());
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }


        public void AgregarVisita(string idUsuario, string idProducto)
        {

        }

        public void AgregarFavorito(string idUsuario)
        {
           
        }
    }
} 
        
