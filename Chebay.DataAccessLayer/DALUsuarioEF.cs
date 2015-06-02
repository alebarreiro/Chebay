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

        #region usuarios
        //--USUARIOS--
        public void AgregarUsuario(Usuario u, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    u.fecha_ingreso = DateTime.Today;
                    u.promedio_calificacion = 0;
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
                    user.Apellido = u.Apellido;
                    user.Nombre = u.Nombre;
                    user.Ciudad = u.Ciudad;
                    user.Pais = u.Pais;
                    user.Direccion = u.Direccion;
                    user.NumeroContacto = u.NumeroContacto;
                    user.CodigoPostal = u.CodigoPostal;
                    user.Email = u.Email;
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

        public List<Usuario> ObtenerTodosUsuariosFull(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var query = from usr in context.usuarios
                                .Include("publicados")
                                .Include("visitas")
                                .Include("favoritos")
                                .Include("ofertas")
                                .Include("compras")
                                .Include("comentarios")
                                .Include("calificaciones")
                                .Include("calificacionesrecibidas")
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

        public Usuario ObtenerUsuarioFull(string idUsuario, string idTienda)
        {
            try
            {
                if (idUsuario == null)
                    throw new Exception("Debe pasar el identificador de un usuario.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var query = from usr in context.usuarios
                                .Include("publicados")
                                .Include("visitas")
                                .Include("favoritos")
                                .Include("ofertas")
                                .Include("compras")
                                .Include("comentarios")
                                .Include("calificaciones")
                                .Include("calificacionesrecibidas")
                                where usr.UsuarioID == idUsuario
                                select usr;
                    if (query.Count() == 0)
                        throw new Exception("No existe el usuario " + idUsuario +".");
                    return query.FirstOrDefault();
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
        #endregion

        #region calificaciones
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
                    var qCalif = from clf in context.usuarios.Include("calificacionesrecibidas")
                                 where clf.UsuarioID == idUsuario
                                 select clf;
                    Usuario u = qCalif.FirstOrDefault();
                    List<Calificacion> CalificacionesUsuario = u.calificacionesrecibidas.ToList();
                    DataCalificacion ret = new DataCalificacion 
                    { 
                        promedio = 0, 
                        cantCalificaciones = 0,
                        cant1 = 0,
                        cant2 = 0,
                        cant3 = 0,
                        cant4 = 0,
                        cant5 = 0
                    };
                    double prom = 0;
                    foreach (Calificacion c in CalificacionesUsuario)
                    {
                        prom += c.puntaje;
                        switch (c.puntaje)
                        {
                            case 1:
                                ret.cant1++;
                                break;
                            case 2:
                                ret.cant2++;
                                break;
                            case 3:
                                ret.cant3++;
                                break;
                            case 4:
                                ret.cant4++;
                                break;
                            case 5:
                                ret.cant5++;
                                break;
                        }
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

        public DataPuedoCalificar PuedoCalificar(long idProducto, string idUsuario, string idTienda)
        {
            try
            {
                if (idProducto == 0)
                    throw new Exception("Debe pasar el identificador de un producto.");
                if (idUsuario == null)
                    throw new Exception("Debe pasar el identificador de un usuario.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    DataPuedoCalificar dpc = new DataPuedoCalificar();
                    
                    //Verifica si idUsuario compró idProducto
                    var qCompra = from cmp in context.compras
                                  where cmp.ProductoID == idProducto
                                  select cmp;
                    if (qCompra.Count() == 0)
                        throw new Exception("Nadie ha comprado el producto " + idProducto + ".");
                    
                    Compra c = qCompra.FirstOrDefault();

                    //Si idUsuario NO realizó la compra, devuelve false.
                    if (c.UsuarioID != idUsuario)
                    {
                        dpc.puedoCalificar = false;
                        return dpc;
                    }
                    
                    //Si idUsuario SI realizó la compra...

                    //Chequeo que no haya realizado la calificación.
                    var qCalif = from clf in context.calificaciones
                                 where clf.ProductoID == idProducto
                                 where clf.UsuarioEvalua == idUsuario
                                 select clf;
                    //Si idUsuario ya realizó la calificación, no puede volver a calificar.
                    if (qCalif.Count() > 0)
                    {
                        dpc.puedoCalificar = false;
                        return dpc;
                    }
                    
                    //Si idUsuario es el comprador y no realizó la calificación.
                    dpc.puedoCalificar = true;
                    
                    //Obtengo el id del vendedor.
                    var qProd = from prd in context.productos
                                where prd.ProductoID == idProducto
                                select prd;
                    if (qProd.Count() == 0)
                        throw new Exception("No existe el producto " + idProducto + ".");

                    Producto p = qProd.FirstOrDefault();
                    dpc.idVendedor = p.UsuarioID;
                    return dpc;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }
        #endregion

        #region imagenes
        //--IMAGENES--
        public void AgregarImagenUsuario(ImagenUsuario iu, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qImagen = from img in context.imagenesusuario
                                  where img.UsuarioID == iu.UsuarioID
                                  select img;
                    if (qImagen.Count() == 0)
                    { //Agregar Imágen
                        context.imagenesusuario.Add(iu);
                        context.SaveChanges();
                    }
                    else
                    { //Modificar imágen
                        ImagenUsuario imgu = qImagen.FirstOrDefault();
                        imgu.Imagen = iu.Imagen;
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

        public ImagenUsuario ObtenerImagenUsuario(string idUsuario, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qImg = from img in context.imagenesusuario
                               where img.UsuarioID == idUsuario
                               select img;
                    ImagenUsuario ret = qImg.FirstOrDefault();
                    return ret;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void EliminarImagenUsuario(string idUsuario, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qImg = from img in context.imagenesusuario
                               where img.UsuarioID == idUsuario
                               select img;
                    ImagenUsuario ret = qImg.FirstOrDefault();
                    context.imagenesusuario.Remove(ret);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }
        #endregion

        #region recomendaciones

        public DataRecomendacion ObtenerRecomendacionesUsuario(string TiendaID, DataRecomendacion dataRecomendacion)
        {
            MongoDB db = new MongoDB();
            return db.GetRecomendacionesUsuario(TiendaID, dataRecomendacion);
        }

        public async Task AgregarRecomendacionesUsuario(string TiendaID, DataRecomendacion dataRecomendacion)
        {
            MongoDB db = new MongoDB();
            await db.InsertProducts(TiendaID, dataRecomendacion);
        }

        public async Task InicializarColeccionRecomendaciones(string TiendaID)
        {
            MongoDB db = new MongoDB();
            await db.createIndexRecomendation(TiendaID);
        }



        #endregion

      
    }
} 
        
