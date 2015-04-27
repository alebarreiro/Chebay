using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DALSubastaEF : IDALSubasta
    {
        void AgregarProducto(Producto p);

        void ModificarProducto(Producto p);

        //void AgregarImagenProducto(Imagen img, Producto p);

        //void AgregarComentarioProducto(Comentario, Producto p);

        Producto ObtenerProducto(int id);

        List<Producto> ObtenerProductosCategoria(string nombreTienda, string nombreCategoria)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from c in context.categorias
                                where c.CategoriaID == nombreCategoria
                                select c;
                    if (query == null)
                        return null;
                    else
                    {
                        CategoriaSimple cs = (CategoriaSimple)query.FirstOrDefault();
                        return cs.productos.ToList();
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        List<Producto> ObtenerProductosVisitados(string idUsuario)
        {
            using (var context = new ChebayDBContext())
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
                        List<Visita> lv = (List<Visita>)u.visitas;
                        List<Producto> ret = new List<Producto>();
                        foreach(Visita v in lv)
                        {
                            ret.Add(v.producto);
                        }
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


        List<Producto> ObtenerProductosFavoritos(string idUsuario)
        {
            using (var context = new ChebayDBContext())
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
                        List<Favorito> lf = (List<Favorito>)u.favoritos;
                        List<Producto> ret = new List<Producto>();
                        foreach (Favorito f in lf)
                        {
                            ret.Add(f.producto);
                        }
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

        List<Producto> ObtenerProductosComprados(string idUsuario)
        {
            using (var context = new ChebayDBContext())
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
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        List<Producto> ObtenerProductosOfertados(string idUsuario)
        {
            using (var context = new ChebayDBContext())
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
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        List<Producto> ObtenerProductosPublicados(string idUsuario)
        {
            using (var context = new ChebayDBContext())
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
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }
    }
}
