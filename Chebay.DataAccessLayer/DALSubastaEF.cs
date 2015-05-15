﻿using Shared.Entities;
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
                if (p == null)
                    throw new Exception("Debe pasar un producto.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    context.productos.Add(p);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }


        public Producto ObtenerProducto(long idProducto, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var prod = from p in context.productos
                               where p.ProductoID == idProducto
                               select p;    
                    return prod.FirstOrDefault();   
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<DataProducto> ObtenerProductosPersonalizados(string idTienda)
        //Devuelve los últimos 10 productos publicados para el index.
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qProductos = from p in context.productos
                                     orderby p.fecha_cierre ascending
                                     select p;
                    List<DataProducto> ret = new List<DataProducto>();
                    if (qProductos.Count() > 0)
                    {
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

        public List<Producto> ObtenerProductosCategoria(long idCategoria, string idTienda)
        {
            try
            {
                if (idCategoria == 0)
                    throw new Exception("Debe pasar el identificador de una categoría.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
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
                throw e;
            }
        }

        public List<Producto> ObtenerProductosVisitados(string idUsuario, string idTienda)
        {
            try
            {
                if (idUsuario == null)
                    throw new Exception("Debe pasar el identificador de un usuario.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }


        public List<Producto> ObtenerProductosFavoritos(string idUsuario, string idTienda)
        {
            try
            {
                if (idUsuario == null)
                    throw new Exception("Debe pasar el identificador de un usuario.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Producto> ObtenerProductosComprados(string idUsuario, string idTienda)
        {
            try
            {
                if (idUsuario == null)
                    throw new Exception("Debe pasar el identificador de un usuario.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Producto> ObtenerProductosOfertados(string idUsuario, string idTienda)
        {
            try
            {
                if (idUsuario == null)
                    throw new Exception("Debe pasar el identificador de un usuario.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Producto> ObtenerProductosPublicados(string idUsuario, string idTienda)
        {
            try
            {
                if (idUsuario == null)
                    throw new Exception("Debe pasar el identificador de un usuario.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void AgregarComentario(Comentario c, string idTienda)
        {
            try
            {
                if (c == null)
                    throw new Exception("Debe pasar un comentario.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    context.comentarios.Add(c);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void OfertarProducto(Oferta o, string idTienda)
        {
            try
            {
                if (o == null)
                    throw new Exception("Debe pasar una oferta.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    context.ofertas.Add(o);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<DataProducto> ObtenerProductosPorTerminar(int cantProductos, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qProd = from p in context.productos
                                orderby p.fecha_cierre
                                where p.fecha_cierre > DateTime.Today
                                select p;
                    List<DataProducto> ret = new List<DataProducto>();
                    List<Producto> aux = qProd.ToList();
                    int cantActual = 0;
                    foreach (Producto p in aux)
                    {
                        cantActual++;
                        DataProducto dp = new DataProducto 
                        {
                            nombre=p.nombre,
                            descripcion=p.descripcion,
                            precio_actual=p.precio_base_subasta,
                            precio_base_subasta=p.precio_base_subasta,
                            precio_compra=p.precio_compra,
                            ProductoID=p.ProductoID,
                            fecha_cierre=p.fecha_cierre
                        };
                        Oferta of = ObtenerMayorOferta(p.ProductoID, idTienda);
                        if (of != null)
                        {
                            dp.precio_actual = of.monto;
                            dp.idOfertante = of.UsuarioID;
                        }
                        ret.Add(dp);
                        if (cantActual == cantProductos)
                            break;
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

        public Oferta ObtenerMayorOferta(long idProducto, string idTienda)
        {
            try
            {
                if (idProducto == null)
                    throw new Exception("Debe pasar el identificador de un producto.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
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
                                  where t.TiendaID == idTienda
                                  select t;
                    if (qTienda.Count() == 0)
                        throw new Exception("No existe la tienda " + idTienda);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Producto ObtenerInfoProducto(long idProducto, string idTienda, string idUsuario)
        {
        //COMENTARIOS, OFERTAS, CATEGORIAS, ATRIBUTOS
            try
            {
                if (idProducto == 0)
                    throw new Exception("Debe pasar el identificador de un producto.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qProducto = from prod in context.productos.Include("comentarios").Include("ofertas").Include("categorias").Include("atributos")
                                    where prod.ProductoID == idProducto
                                    select prod;
                    Producto ret = qProducto.FirstOrDefault();

                    var qUserProducto = from usr in context.usuarios
                                        where usr.UsuarioID == ret.UsuarioID
                                        select usr;
                    ret.usuario = qUserProducto.FirstOrDefault();

                    //AGREGAR VISITA DE PRODUCTO
                    if (idUsuario != null)
                    { 
                        var qUserVisita = from usr in context.usuarios
                                          where usr.UsuarioID == ret.UsuarioID
                                          select usr;
                        if (qUserVisita.Count() > 0)
                        {
                            if (ret.visitas == null)
                                ret.visitas = new HashSet<Usuario>();
                            ret.visitas.Add(qUserVisita.FirstOrDefault());
                            context.SaveChanges();
                        }
                    }

                    //CALCULAR PROMEDIO DE CALIFICACIONES DE UN USUARIO.
                   /* var qCalificaciones = from cal in context.calificaciones
                                          where cal.UsuarioCalificado == ret.UsuarioID
                                          select cal;
                    List<Calificacion> lCalificaciones = qCalificaciones.ToList();
                    double promedio = 0;
                    foreach (Calificacion c in lCalificaciones)
                    {
                        promedio += c.puntaje;
                    }
                    promedio = promedio / lCalificaciones.Count;
                    */
                    return ret;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
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
                throw e;
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
                throw e;
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
                throw e;
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
                throw e;
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
                throw e;
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
                throw e;
            }
        }

    }
}
