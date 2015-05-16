using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using Shared.DataTypes;

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

        //--USUARIOS--
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
                    if (query.Count() == 0)
                        throw new Exception("No existe el usuario.");
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

        //--CALIFICACIONES--
        public void AgregarCalificacion(Calificacion c, string idTienda)
        {
            try
            {
                if (c == null)
                    throw new Exception("Debe pasar una calificación.");
                if (c.puntaje < 1 || c.puntaje > 5)
                    throw new Exception("El puntaje debe ser entre 1 y 5.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    context.calificaciones.Add(c);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Calificacion ObtenerCalificacion(long idCalificacion, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCalif = from clf in context.calificaciones
                                 where clf.ID == idCalificacion
                                 select clf;
                    return qCalif.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Calificacion> ObtenerCalificaciones(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCalif = from clf in context.calificaciones
                                 select clf;
                    return qCalif.ToList();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void EliminarCalificacion(long idCalificacion, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCalif = from clf in context.calificaciones
                                 where clf.ID == idCalificacion
                                 select clf;
                    Calificacion c = qCalif.FirstOrDefault();
                    context.calificaciones.Remove(c);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public DataCalificacion ObtenerCalificacionUsuario(string idUsuario, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCalif = from clf in context.calificaciones
                                 where clf.UsuarioCalificado == idUsuario
                                 select clf;
                    List<Calificacion> CalificacionesUsuario = qCalif.ToList();
                    DataCalificacion ret = new DataCalificacion { promedio = 0, cantCalificaciones = 0 };
                    double prom = 0;
                    foreach (Calificacion c in CalificacionesUsuario)
                    {
                        prom += c.puntaje;
                    }
                    if (CalificacionesUsuario.Count > 0)
                    {
                        prom = prom / CalificacionesUsuario.Count;
                        ret.promedio = prom;
                        ret.cantCalificaciones = CalificacionesUsuario.Count;
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
        
