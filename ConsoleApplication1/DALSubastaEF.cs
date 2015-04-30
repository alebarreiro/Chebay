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
        public void AgregarProducto(string idUsuario, string nomProducto, string descProducto, int precioBase, int precioCompra, DateTime fechaCierre, string idCategoria)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var usr = from u in context.usuarios
                              where u.UsuarioID == idUsuario
                              select u;
                    var cat = from c in context.categorias
                              where c.CategoriaID == idCategoria
                              select c;
                    if (usr == null || cat == null)
                        throw new Exception("No existe el usuario o la categoría.");
                    else
                    {
                        Producto p = new Producto();
                        p.UsuarioID = idUsuario;
                        p.CategoriaID = idCategoria;
                        p.nombre = nomProducto;
                        p.descripcion = descProducto;
                        p.precio_base_subasta = precioBase;
                        p.precio_compra = precioCompra;
                        p.fecha_cierre = fechaCierre;
                        Usuario user = usr.FirstOrDefault();
                        p.usuario = user;
                        CategoriaSimple c = (CategoriaSimple)cat.FirstOrDefault();
                        p.categoria = c;
                        c.productos.Add(p);
                        user.publicados.Add(p);
                        context.productos.Add(p);
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        public void ModificarProducto(long idProducto, string nomProducto, string descProducto, int precioBase, int precioCompra, DateTime fechaCierre, string idCategoria)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var prd = from p in context.productos
                              where p.ProductoID == idProducto
                              select p;
                    var catNueva = from c in context.categorias
                              where c.CategoriaID == idCategoria
                              select c;
                    if (catNueva == null)
                        throw new Exception("No existe el usuario o la categoría.");
                    else
                    {
                        Producto p = prd.FirstOrDefault();
                        
                        p.nombre = nomProducto;
                        p.descripcion = descProducto;
                        p.precio_base_subasta = precioBase;
                        p.precio_compra = precioCompra;
                        p.fecha_cierre = fechaCierre;

                        CategoriaSimple cNueva = (CategoriaSimple)catNueva.FirstOrDefault();
                        if (p.CategoriaID != cNueva.CategoriaID)
                        {
                            p.CategoriaID = idCategoria;
                            p.categoria = cNueva;

                        }
                        
                        //c.productos.Add(p);
                        context.productos.Add(p);
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        //void AgregarImagenProducto(Imagen img, Producto p);

        //void AgregarComentarioProducto(Comentario, Producto p);

        public Producto ObtenerProducto(long idProducto, string idUsuario)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var prod = from p in context.productos
                               where p.ProductoID == idProducto
                               select p;
                    var usr = from u in context.usuarios
                              where u.UsuarioID == idUsuario
                              select u;

                    if (prod.Count() > 0 && usr.Count() > 0)
                    {
                        Producto pr = prod.FirstOrDefault();
                        Usuario user = usr.FirstOrDefault();
                        Visita v = new Visita();
                        v.ProductoID = idProducto;
                        v.producto = pr;
                        v.UsuarioID = idUsuario;
                        v.usuario = user;
                        pr.visitas.Add(v);
                        user.visitas.Add(v);
                        context.visitas.Add(v);
                        context.SaveChanges(); 
                        return pr;
                    }
                    else
                        return null;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public List<Producto> ObtenerProductosCategoria(string nombreCategoria)
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

        public List<Producto> ObtenerProductosVisitados(string idUsuario)
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


        public List<Producto> ObtenerProductosFavoritos(string idUsuario)
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

        public List<Producto> ObtenerProductosComprados(string idUsuario)
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

        public List<Producto> ObtenerProductosOfertados(string idUsuario)
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

        public List<Producto> ObtenerProductosPublicados(string idUsuario)
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

        public void AgregarComentario(string texto, long idProducto, string idUsuario)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var prod = from p in context.productos
                               where p.ProductoID == idProducto
                               select p;
                    var user = from u in context.usuarios
                               where u.UsuarioID == idUsuario
                               select u;
                    if (prod != null && user != null)
                    {
                        Usuario u = user.FirstOrDefault();
                        Producto p = prod.FirstOrDefault();
                        Comentario c = new Comentario();
                        c.fecha = (DateTime)System.DateTime.Now;
                        c.ProductoID = idProducto;
                        c.UsuarioID = idUsuario;
                        c.usuario = u;
                        c.producto = p;
                        c.texto = texto;
                        context.comentarios.Add(c);
                        u.comentarios.Add(c);
                        p.comentarios.Add(c);
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        public void OfertarProducto(string idOfertante, long idProducto, double monto)
        {

        }

    }
}
