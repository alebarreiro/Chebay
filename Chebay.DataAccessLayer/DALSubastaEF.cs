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
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
                    context.productos.Add(p);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }


        public Producto ObtenerProducto(long idProducto, string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
                    var prod = from p in context.productos
                               where p.ProductoID == idProducto
                               select p;    
                    return prod.FirstOrDefault();   
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public List<DataProducto> ObtenerProductosPersonalizados(string urlTienda)
        //Devuelve los últimos 10 productos publicados para el index.
        {
            using (var context = ChebayDBContext.CreateTenant(urlTienda))
            {
                try
                {
                    var qProductos = from p in context.productos
                                     select p;
                    if (qProductos.Count() > 0)
                    {
                        List<DataProducto> ret = new List<DataProducto>();
                        foreach (Producto p in qProductos)
                        {
                            DataProducto dp = new DataProducto();
                            dp.descripcion = p.descripcion;
                            dp.fecha_cierre = p.fecha_cierre;
                            dp.nombre = p.nombre;
                            dp.precio_base_subasta = p.precio_base_subasta;
                            dp.precio_compra = p.precio_compra;
                            ret.Add(dp);
                        }
                        return ret;
                    }
                    else
                        throw new Exception("No hay productos.");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public List<Producto> ObtenerProductosCategoria(long idCategoria, string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
                    var query = from c in context.categorias
                                where c.CategoriaID == idCategoria
                                select c;
                    CategoriaSimple cs = (CategoriaSimple)query.FirstOrDefault();
                    return cs.productos.ToList();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public List<Producto> ObtenerProductosVisitados(string idUsuario, string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
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
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }


        public List<Producto> ObtenerProductosFavoritos(string idUsuario, string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
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
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public List<Producto> ObtenerProductosComprados(string idUsuario, string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
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
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public List<Producto> ObtenerProductosOfertados(string idUsuario, string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
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
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public List<Producto> ObtenerProductosPublicados(string idUsuario, string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
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
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public void AgregarComentario(Comentario c, string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                try
                {
                    context.comentarios.Add(c);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public void OfertarProducto(string idOfertante, long idProducto, double monto)
        {

        }
    }
}
