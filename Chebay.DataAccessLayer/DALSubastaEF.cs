using Shared.Entities;
using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data.Entity.Validation;

namespace DataAccessLayer
{
    public class DALSubastaEF : IDALSubasta
    {
        public void AgregarProducto(Producto p, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    context.productos.Add(p);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }


        public Producto ObtenerProducto(long idProducto, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    var prod = from p in context.productos
                               where p.ProductoID == idProducto
                               select p;    
                    return prod.FirstOrDefault();   
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public List<DataProducto> ObtenerProductosPersonalizados(string idTienda)
        //Devuelve los últimos 10 productos publicados para el index.
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    var qProductos = from p in context.productos
                                     orderby p.fecha_cierre descending
                                     select p;
                    if (qProductos.Count() > 0)
                    {
                        List<DataProducto> ret = new List<DataProducto>();
                        foreach (Producto p in qProductos)
                        {
                            DataProducto dp = new DataProducto { 
                                descripcion = p.descripcion,
                                fecha_cierre = p.fecha_cierre,
                                nombre = p.nombre, 
                                precio_base_subasta = p.precio_base_subasta,
                                precio_actual = p.precio_base_subasta,
                                precio_compra = p.precio_compra, 
                                ProductoID = p.ProductoID,
                                idOfertante = null
                            };
                            Oferta of = ObtenerMayorOferta(p.ProductoID,idTienda);
                            if (of != null)
                            {
                                dp.precio_actual = of.monto;
                                dp.idOfertante = of.UsuarioID;
                            }
                            ret.Add(dp);
                        }
                        return ret;
                    }
                    else
                        throw new Exception("No hay productos.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public List<Producto> ObtenerProductosCategoria(long idCategoria, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    var query = from c in context.categorias
                                where c.CategoriaID == idCategoria
                                select c;
                    CategoriaSimple cs = (CategoriaSimple)query.FirstOrDefault();
                    return cs.productos.ToList();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public List<Producto> ObtenerProductosVisitados(string idUsuario, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    var query = from u in context.usuarios
                                where u.UsuarioID == idUsuario
                                select u;
                    if (query == null)
                        return null;
                    else
                    {
                        Usuario u = query.FirstOrDefault();
                        List<Producto> ret = u.visitas.ToList();
                        return ret;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }


        public List<Producto> ObtenerProductosFavoritos(string idUsuario, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    var query = from u in context.usuarios
                                where u.UsuarioID == idUsuario
                                select u;
                    if (query == null)
                        return null;
                    else
                    {
                        Usuario u = query.FirstOrDefault();
                        return u.favoritos.ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public List<Producto> ObtenerProductosComprados(string idUsuario, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    var query = from u in context.usuarios
                                where u.UsuarioID == idUsuario
                                select u;
                    if (query == null)
                        return null;
                    else
                    {
                        Usuario u = query.FirstOrDefault();
                        List<Compra> lc = (List<Compra>)u.compras;
                        List<Producto> ret = new List<Producto>();
                        foreach (Compra c in lc)
                        {
                            ret.Add(c.producto);
                        }
                        return ret;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public List<Producto> ObtenerProductosOfertados(string idUsuario, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    var query = from u in context.usuarios
                                where u.UsuarioID == idUsuario
                                select u;
                    if (query == null)
                        return null;
                    else
                    {
                        Usuario u = query.FirstOrDefault();
                        List<Oferta> lo = (List<Oferta>)u.ofertas;
                        List<Producto> ret = new List<Producto>();
                        foreach (Oferta o in lo)
                        {
                            ret.Add(o.producto);
                        }
                        return ret;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public List<Producto> ObtenerProductosPublicados(string idUsuario, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    var query = from u in context.usuarios
                                where u.UsuarioID == idUsuario
                                select u;
                    if (query == null)
                        return null;
                    else
                    {
                        Usuario u = query.FirstOrDefault();
                        return (List<Producto>)u.publicados;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public void AgregarComentario(Comentario c, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    //chequearTienda(idTienda);
                    context.comentarios.Add(c);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public void OfertarProducto(Oferta o, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    context.ofertas.Add(o);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public List<DataProducto> ObtenerProductosPorTerminar(string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public Oferta ObtenerMayorOferta(long idProducto, string idTienda)
        {
            try
            {
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    chequearTienda(idTienda);
                    var qOfertas = from o in context.ofertas
                                   where o.ProductoID == idProducto
                                   orderby o.monto descending
                                   select o;
                    return qOfertas.FirstOrDefault();
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
                using (var context = ChebayDBPublic.CreatePublic())
                {
                    var qTienda = from t in context.tiendas
                                  where t.TiendaID == idTienda
                                  select t;
                    if (qTienda.Count() == 0)
                        throw new Exception("No existe la tienda " + idTienda);
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
