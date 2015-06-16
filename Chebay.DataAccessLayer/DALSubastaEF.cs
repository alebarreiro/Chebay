using Microsoft.ServiceBus.Messaging;
using Shared.DataTypes;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading;

namespace DataAccessLayer
{
    public class DALSubastaEF : IDALSubasta
    {

        #region productos
        //--PRODUCTOS--
        public long AgregarProducto(Producto p, string idTienda)
        {
            try
            {
                if (p == null)
                    throw new Exception("Debe pasar un producto.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                
                    //Agrega el producto a la lista de publicados del usuario.
                    var qUser = from usr in context.usuarios
                                where usr.UsuarioID == p.UsuarioID
                                select usr;
                    if (qUser.Count() == 0)
                        throw new Exception("No existe el usuario " + p.UsuarioID);

                    Usuario u = qUser.FirstOrDefault();
                    if (u.publicados == null)
                        u.publicados = new HashSet<Producto>();
                    u.publicados.Add(p);
                
                    //Agregar el producto a la lista de productos de la Categoria.
                    var qCat = from cat in context.categorias
                               where cat.CategoriaID == p.CategoriaID
                               select cat;
                    if (qCat.Count() == 0)
                        throw new Exception("No existe la categoria " + p.CategoriaID);

                    CategoriaSimple cs = (CategoriaSimple)qCat.FirstOrDefault();
                    if (cs.productos == null)
                        cs.productos = new HashSet<Producto>();
                    cs.productos.Add(p);
                
                    context.productos.Add(p);
                    context.SaveChanges();

                    //app.config...

                    string QueueName = "subasta";
                    string connectionString = "Endpoint=sb://chebay.servicebus.windows.net/;SharedAccessKeyName=auth;SharedAccessKey=Jd/ztAsr+7snQ02QpUfn9bIvb9QvTjup+nox7GDw1dM=";
                    //CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
                   
                    //mandar a la queue fecha de cierre
                    QueueClient Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
                    //creo dataproductoqueue
                    DataProductoQueue dpq = new DataProductoQueue { OwnerProducto=u.UsuarioID, nombre = p.nombre, fecha_cierre= p.fecha_cierre, ProductoID=p.ProductoID, TiendaID=idTienda };

                    var message = new BrokeredMessage(dpq) { ScheduledEnqueueTimeUtc = p.fecha_cierre };
                    Client.Send(message);
                    System.Console.WriteLine(p.fecha_cierre.ToString());

                    return p.ProductoID;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Producto> ObtenerTodosProductos(string idTienda)
        {
            using (var context = ChebayDBContext.CreateTenant(idTienda))
            {
                var query = from p in context.productos
                                //.Include("usuario")//.Include("categoria")
                                .Include("visitas")
                                .Include("favoritos")
                                .Include("ofertas")
                                .Include("comentarios")
                            select p;
                return query.ToList();
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
        #endregion

        #region varios obtener productos

        public bool TieneOfertas(long ProductoID, string TiendaID)
        {
            using (var db = ChebayDBContext.CreateTenant(TiendaID))
            {
                var query = from p in db.productos.Include("ofertas")
                            where p.ProductoID == ProductoID
                            select p;
                Producto prod = query.First();
                if (prod.ofertas.Count() > 0)
                {
                    return true;
                }
            }
            return false;
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
                    var qProducto = from prod in context.productos.Include("comentarios").Include("ofertas").Include("atributos")
                                    where prod.ProductoID == idProducto
                                    select prod;
                    Producto ret = qProducto.FirstOrDefault();

                    var qUserProducto = from usr in context.usuarios
                                        where usr.UsuarioID == ret.UsuarioID
                                        select usr;
                    ret.usuario = qUserProducto.FirstOrDefault();

                    var qImagenes = from img in context.imagenesproducto
                                    where img.ProductoID == idProducto
                                    select img;
                    List<ImagenProducto> lip = qImagenes.ToList();
                    if (ret.imagenes == null)
                        ret.imagenes = new HashSet<ImagenProducto>();
                    foreach (ImagenProducto ip in lip)
                    {
                        ret.imagenes.Add(ip);
                    }


                    //AGREGAR VISITA DE PRODUCTO
                    if (idUsuario != null)
                    {
                        var qUserVisita = from usr in context.usuarios.Include("visitas")
                                          where usr.UsuarioID == ret.UsuarioID
                                          select usr;
                        if (qUserVisita.Count() > 0) //Si el usuario que visita el producto existe...
                        {
                            Usuario u = qUserVisita.FirstOrDefault();
                            if (!u.visitas.Contains(ret)) //Si es la primera vez que visita el producto, agrega la visita.
                            {
                                if (ret.visitas == null)
                                    ret.visitas = new HashSet<Usuario>();
                                ret.visitas.Add(qUserVisita.FirstOrDefault());
                                context.SaveChanges();
                            }
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

        public List<DataProducto> ObtenerProductosPersonalizados(string idTienda)
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

       
        public List<DataProducto> ObtenerProductosCategoria(long idCategoria, string idTienda)
        {
            IDALTienda idalt = new DALTiendaEF();
            List<DataProducto> listadp = new List<DataProducto>();
            

            //obtengo todas las categorias simples de idCategoria
            using(var db = ChebayDBContext.CreateTenant(idTienda))
            {         
                //valido
                var query = from cat in db.categorias
                            where cat.CategoriaID == idCategoria
                            select cat;
                if(query.Count()==0){
                    throw new Exception("DALSubastaEF::ObtenerProductosCategoria: No existe categoria");
                }
                
                //backtracking
                List<Categoria> stack = new List<Categoria>();
                stack.Add(query.First());

                while (stack.Count() != 0)
                {
                    var first = stack.First();
                    System.Console.WriteLine(first.CategoriaID);

                    if (first is CategoriaSimple)
                    {
                        CategoriaSimple cs = (CategoriaSimple)first;
                        db.Entry(cs).Collection(c => c.productos).Load();
                        
                        foreach (var p in cs.productos)
                        {
                            //Para agregar solo los productos que no vencieron.
                            if (p.fecha_cierre > DateTime.Now)
                            {
                                DataProducto dp = new DataProducto
                                {
                                    nombre = p.nombre,
                                    descripcion = p.descripcion,
                                    precio_actual = p.precio_base_subasta,
                                    precio_base_subasta = p.precio_base_subasta,
                                    precio_compra = p.precio_compra,
                                    ProductoID = p.ProductoID,
                                    fecha_cierre = p.fecha_cierre
                                };
                                //Para agregar idOfertante y mayor oferta recibida
                                Oferta of = ObtenerMayorOferta(p.ProductoID, idTienda);
                                if (of != null)
                                {
                                    dp.precio_actual = of.monto;
                                    dp.idOfertante = of.UsuarioID;
                                }
                                listadp.Add(dp);
                            }
                        }
                    }
                    else
                    {
                        CategoriaCompuesta cc = (CategoriaCompuesta)first;
                        db.Entry(cc).Collection(p => p.hijas).Load();
                        
                        foreach (var c in cc.hijas)
                        {
                            stack.Add(c);
                        }
                    }
                    //quito el primero
                    stack.RemoveAt(0);
                }
                
            }//using

            return listadp;
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

        public List<DataProducto> ObtenerProductosPorTerminar(int cantProductos, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qProd = from p in context.productos
                                orderby p.fecha_cierre
                                where p.fecha_cierre > DateTime.Now
                                select p;
                    List<DataProducto> ret = new List<DataProducto>();
                    List<Producto> aux = qProd.ToList();
                    int cantActual = 0;
                    foreach (Producto p in aux)
                    {
                        cantActual++;
                        DataProducto dp = new DataProducto
                        {
                            nombre = p.nombre,
                            descripcion = p.descripcion,
                            precio_actual = p.precio_base_subasta,
                            precio_base_subasta = p.precio_base_subasta,
                            precio_compra = p.precio_compra,
                            ProductoID = p.ProductoID,
                            fecha_cierre = p.fecha_cierre
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

        public List<DataProducto> ObtenerProductosBuscados(string searchTerm, string idTienda)
        {
            try
            {
                if (searchTerm == null)
                    throw new Exception("Debe pasar el algo para buscar.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qProductos = from prd in context.productos
                                     select prd;
                    List<Producto> lp = qProductos.ToList();
                    List<DataProducto> ret = new List<DataProducto>();
                    foreach (Producto p in lp)
                    {
                        if (p.nombre.ToLower().Contains(searchTerm.ToLower()) ||
                            ( 
                                p.descripcion != null && 
                                p.descripcion.ToLower().Contains(searchTerm.ToLower())
                            ) ||
                            p.UsuarioID.ToLower().Contains(searchTerm.ToLower()))
                        {
                            DataProducto dp = new DataProducto
                            {
                                descripcion = p.descripcion,
                                fecha_cierre = p.fecha_cierre,
                                nombre = p.nombre,
                                precio_base_subasta = p.precio_base_subasta,
                                precio_actual = p.precio_base_subasta,
                                precio_compra = p.precio_compra,
                                ProductoID = p.ProductoID,
                                idOfertante = null
                            };
                            Oferta of = ObtenerMayorOferta(p.ProductoID, idTienda);
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
        
        public List<Producto> ObtenerProductosVencidos(DateTime ini, DateTime fin, string idTienda)
        {
            try
            {
                if (ini == null || fin == null)
                    throw new Exception("La fecha de inicio o de fin es inválida.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qProducto = from prod in context.productos.Include("ofertas")
                                    where prod.fecha_cierre > ini
                                    where prod.fecha_cierre < fin
                                    select prod;
                    List<Producto> ret = qProducto.ToList();
                    return ret;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }
        
        #endregion

        #region comentarios
        //--COMENTARIO--
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

        public Comentario ObtenerComentario(long idComentario, string idTienda)
        {
            try
            {
                if (idComentario == 0)
                    throw new Exception("Debe pasar el identificador de un comentario.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qComentario = from com in context.comentarios
                                      where com.ComentarioID == idComentario
                                      select com;
                    return qComentario.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Comentario> ObtenerComentarios(long idProducto, string idTienda)
        {
            try
            {
                if (idProducto == 0)
                    throw new Exception("Debe pasar el identificador de un producto.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qComentarios = from com in context.comentarios
                                       where com.ProductoID == idProducto
                                       select com;
                    return qComentarios.ToList();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Comentario> ObtenerComentarios(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qComentarios = from com in context.comentarios
                                       select com;
                    return qComentarios.ToList();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void EliminarComentario(long idComentario, string idTienda)
        {
            try
            {
                if (idComentario == 0)
                    throw new Exception("Debe pasar el identificador de un comentario.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qComentario = from com in context.comentarios
                                      where com.ComentarioID == idComentario
                                      select com;
                    Comentario c = qComentario.FirstOrDefault();
                    context.comentarios.Remove(c);
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

        #region ofertas
        public void OfertarProducto(Oferta o, string idTienda)
        {
            try
            {
                if (o == null)
                    throw new Exception("Debe pasar una oferta.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    //Chequeo que no haya Compra para el producto.
                    var qCompra = from cmp in context.compras
                                  where cmp.ProductoID == o.ProductoID
                                  select cmp;
                    if (qCompra.Count() > 0)
                        throw new Exception("El producto " + o.ProductoID + " ya fue comprado.");

                    //Chequeo que la fecha actual < fecha_cierre
                    var qProducto = from prd in context.productos
                                    where prd.ProductoID == o.ProductoID
                                    select prd;
                    if (DateTime.UtcNow > qProducto.FirstOrDefault().fecha_cierre)
                        throw new Exception("El producto " + o.ProductoID + " ya ha expirado.");

                    //Agrego la oferta a la colección del usuario.
                    var qUsuario = from usr in context.usuarios
                                   where usr.UsuarioID == o.UsuarioID
                                   select usr;
                    Usuario u = qUsuario.FirstOrDefault();
                    if (u.ofertas == null)
                        u.ofertas = new HashSet<Oferta>();
                    u.ofertas.Add(o);

                    //Agrego la oferta a la base.
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

        public List<Oferta> ObtenerOfertas(int n, long idProducto, string idTienda)
        {
            try
            {
                if (idProducto == 0)
                    throw new Exception("Debe pasar el identificador de un producto.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qOfertas = from of in context.ofertas
                                   where of.ProductoID == idProducto
                                   orderby of.monto descending
                                   select of;
                    List<Oferta> lo = qOfertas.ToList();
                    if (lo.Count < n)
                        return lo;
                    else
                        return lo.GetRange(0,n);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Oferta> ObtenerOfertas(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qOfertas = from of in context.ofertas
                                   orderby of.monto
                                   select of;
                    return qOfertas.ToList();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Oferta ObtenerOferta(long idOferta, string idTienda)
        {
            try
            {
                if (idOferta == 0)
                    throw new Exception("Debe pasar el identificador de una oferta.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qOfertas = from of in context.ofertas
                                   where of.OfertaID == idOferta
                                   select of;
                    return qOfertas.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Oferta ObtenerOfertaGanadora (long idProducto, string idTienda)
        {
            try
            {
                if (idProducto == 0)
                    throw new Exception("Debe pasar el identificador de un producto.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qOfertas = from of in context.ofertas
                                   where of.ProductoID == idProducto
                                   orderby of.monto descending
                                   select of;
                    return qOfertas.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void EliminarOferta(long idOferta, string idTienda)
        {
            try
            {
                if (idOferta == 0)
                    throw new Exception("Debe pasar el identificador de una oferta.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qOfertas = from of in context.ofertas
                                   where of.OfertaID == idOferta
                                   select of;
                    Oferta o = qOfertas.FirstOrDefault();
                    context.ofertas.Remove(o);
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

        #region private
        Oferta ObtenerMayorOferta(long idProducto, string idTienda)
        {
            try
            {
                if (idProducto == 0)
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
        #endregion

        #region favoritos
        public void AgregarFavorito(long idProducto, string idUsuario, string idTienda)
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
                    var qUsuario = from usr in context.usuarios.Include("favoritos")
                                   where usr.UsuarioID == idUsuario
                                   select usr;
                    Usuario u = qUsuario.FirstOrDefault();
                    var qProducto = from prd in context.productos
                                    where prd.ProductoID == idProducto
                                    select prd;
                    Producto p = qProducto.FirstOrDefault();
                    if (u.favoritos == null)
                        u.favoritos = new HashSet<Producto>();
                    u.favoritos.Add(p);
                    if (p.favoritos == null)
                        p.favoritos = new HashSet<Usuario>();
                    p.favoritos.Add(u);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public bool EsFavorito(long idProducto, string idUsuario, string idTienda)
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
                    bool ret = false;
                    var qUsuario = from usr in context.usuarios.Include("favoritos")
                                   where usr.UsuarioID == idUsuario
                                   select usr;
                    Usuario u = qUsuario.FirstOrDefault();

                    var qProducto = from prd in context.productos
                                    where prd.ProductoID == idProducto
                                    select prd;
                    Producto p = qProducto.FirstOrDefault();

                    if (u.favoritos != null)
                        if (u.favoritos.Contains(p))
                            ret = true;
                    return ret;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public int ObtenerCantFavoritos(long idProducto, string idTienda)
        {
            try
            {
                if (idProducto == 0)
                    throw new Exception("Debe pasar el identificador de un producto.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qProducto = from prd in context.productos
                                    where prd.ProductoID == idProducto
                                    select prd;
                    Producto p = qProducto.FirstOrDefault();
                    var qUsuario = from usr in context.usuarios.Include("favoritos")
                                   select usr;
                    List<Usuario> lu = qUsuario.ToList();
                    int ret = 0;
                    foreach (Usuario u in lu)
                    {
                        if (u.favoritos != null)
                            if (u.favoritos.Contains(p))
                                ret++;
                    }
                    Debug.WriteLine(ret);
                    return ret;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void EliminarFavorito(long idProducto, string idUsuario, string idTienda)
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
                    var qUsuario = from usr in context.usuarios.Include("favoritos")
                                   where usr.UsuarioID == idUsuario
                                   select usr;
                    Usuario u = qUsuario.FirstOrDefault();
                    var qProducto = from prd in context.productos
                                    where prd.ProductoID == idProducto
                                    select prd;
                    Producto p = qProducto.FirstOrDefault();
                    u.favoritos.Remove(p);
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

        #region compras
        //--COMPRAS--
        public void AgregarCompra(Compra c, string idTienda)
        {
            try
            {
                if (c == null)
                    throw new Exception("Debe pasar una Compra.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    //Chequea que no haya otras compras para ese producto.
                    var qCompra = from cmp in context.compras
                                  where cmp.ProductoID == c.ProductoID
                                  select cmp;
                    if (qCompra.Count() > 0)
                        throw new Exception("El producto " + c.ProductoID + " ya fue comprado.");

                    //Para que no se puedan hacer más ofertas sobre el producto.
                    var qProducto = from prd in context.productos
                                    where prd.ProductoID == c.ProductoID
                                    select prd;
                    Producto p = qProducto.FirstOrDefault();
                    p.fecha_cierre = DateTime.UtcNow;

                    //Obtengo el usuario que esta comprando.
                    var qUsuario = from usr in context.usuarios
                                   where usr.UsuarioID == c.UsuarioID
                                   select usr;
                    Usuario u = qUsuario.FirstOrDefault();

                    //Actualizo el balance monteario del comprador.
                    u.compras_valor += c.monto;

                    //Obtengo el usuario que esta vendiendo.
                    var qVendedor = from vnd in context.usuarios
                                    where vnd.UsuarioID == p.UsuarioID
                                    select vnd;
                    Usuario v = qVendedor.FirstOrDefault();

                    //Actualizo el balance monteario del comprador.
                    v.ventas_valor += c.monto;

                    //Vinculo la compra con el producto y el usuario.
                    p.compra = c;
                    if (u.compras == null)
                        u.compras = new HashSet<Compra>();
                    u.compras.Add(c);
                    context.compras.Add(c);
                    
                    context.SaveChanges();

                    //Notifico inmediatamente al comprador
                    Thread t = new Thread(delegate()
                    {
                        String linkTienda = "http://chebuynow.azurewebsites.net/"+ idTienda;
                        BLNotificaciones bl = new BLNotificaciones();
                        {
                            string asuntou = "Chebay: Compra de producto!";
                            /*string mensajeu = String.Format("<p>Enhorabuena, haz comprado el producto {0} {1} por el valor de ${2}.</p> <p>Te invitamos a calificar al vendedor accediendo al siguiente enlace :  http://chebuynow.azurewebsites.net/{3}/Usuario/CalificarUsuario?prodId={4} .</p>",
                                p.ProductoID, p.nombre, c.monto, idTienda, p.ProductoID);*/
                            String dest = "";
                            String linkCalif = "http://chebuynow.azurewebsites.net/" + idTienda + "/Usuario/CalificarUsuario?prodId=" + p.ProductoID;

                            if (u.Email != null)
                            {
                                dest = u.Email;
                            }
                            else if (IsValidMail(u.UsuarioID))
                            {
                                dest = u.UsuarioID;
                            }
                            if (dest != "")
                            {
                                string bodyMensaje = getBodyMailHTML(idTienda + ": Subasta ganada!", "Has ganado una nueva subasta!", u, p, c.monto, true, linkCalif, linkTienda);
                                bl.sendEmailNotification(dest, asuntou, bodyMensaje);
                            }
                        }
                        //mail a vendedor
                        //obtengo vendedor
                        IDALUsuario udal = new DALUsuarioEF();
                        Usuario vendedor = udal.ObtenerUsuario(p.UsuarioID, idTienda);
                        string asunto = "Chebay: Venta de producto!";
                        string mensaje = String.Format("<p>El producto {0} {1} se ha vendido al usuario {2} por el valor de ${3}.</p>",
                            p.ProductoID, p.nombre, c.UsuarioID, c.monto);
                        string bodyMensajeVendedor = getBodyMailHTML(idTienda + ": Producto vendido!", "Has vendido un nuevo producto!", u, p, c.monto, false, "", linkTienda);
                        if (vendedor.Email != null)
                        {
                            bl.sendEmailNotification(vendedor.Email, asunto, bodyMensajeVendedor);
                        }
                        else
                        {
                            if (IsValidMail(vendedor.UsuarioID))
                            {
                                bl.sendEmailNotification(vendedor.UsuarioID, asunto, bodyMensajeVendedor);
                            }
                            //else... por algun error no tiene mail
                        }
                    });
                    //asincronismo
                    Debug.WriteLine("Empieza envio mail...");
                    t.Start();
                    Debug.WriteLine("Termina envio de mail...");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public bool IsValidMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /**
         * Función para mandar un mail formato HTML al cliente o al vendedor de producto
         * 
         * {titulo} Subasta ganada! ó Producto vendido!
         * {motivo} Has ganado una nueva subasta ó Has vendido un nuevo producto
         * {u} Usuario vendedor o comprador
         * {p} Producto vendido o comprado
         * {monto} Monto de la compra / venta
         * {calificacion} true si es para el usuario que compró el producto
         * {linkCalif} Si calificacion = true http://chebuynow.azurewebsites.net/{tiendaId}/Usuario/CalificarUsuario?prodId={prodId}
         *             Si calificacion = false ""
         * {linkWeb}  http://chebuynow.azurewebsites.net/tiendaId
         */
        public string getBodyMailHTML(String titulo, String motivo, Usuario u, Producto p, int monto, bool calificacion, String linkCalif, String linkWeb)
        {
            String dest = "";
            String fecha = DateTime.UtcNow.ToString();
            if (u.Nombre != "" || u.Apellido != "") {
                dest = u.Nombre + " " + u.Apellido;
            } else {
                dest = u.UsuarioID;
            }
            String cuerpo = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\n"
                    + "<head>\n"
                    + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n"
                    + "<title>" + titulo +"</title>\n"
                    + "</head>\n"
                    + "\n"
                    + "<body>\n"
                    + "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n"
                    + "  <tr>\n"
                    + "    <td align=\"center\" valign=\"top\" bgcolor=\"#ffe77b\" style=\"background-color:#ffe77b;\"><br>\n"
                    + "    <br>\n"
                    + "    <table width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n"
                    + "      <tr>\n"
                    + "      </tr>\n"
                    + "      <tr>\n"
                    + "        <td align=\"left\" valign=\"top\" bgcolor=\"#f89406\" style=\"background-color:#f89406; font-family:Arial, Helvetica, sans-serif; padding:10px;\"><div style=\"font-size:46px; color:#ff5500;\"><b>" + titulo + "</b></div>\n"
                    + "  <div style=\"font-size:13px; color:#000;\"><b>Estimado cliente " + dest + ", "+ motivo +"</b></div>\n"
                    + "          </td>\n"
                    + "      </tr>\n"
                    + "      <tr>\n"
                    + "        <td align=\"left\" valign=\"top\" bgcolor=\"#ffffff\" style=\"background-color:#ffffff;\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n"
                    + "          <tr>\n"
                    + "            <td align=\"center\" valign=\"middle\" style=\"padding:10px; color:#ff5500; font-size:42px; font-family:Georgia, 'Times New Roman', Times, serif;\">" + p.nombre + "</td>\n"
                    + "          </tr>\n"
                    + "        </table>\n"
                    + "          <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\n"
                    + "            <tr>\n"
                    + "              <td align=\"left\" valign=\"middle\" style=\"color:#525252; font-family:Arial, Helvetica, sans-serif; padding:10px;\">\n"
                    + "              <div style=\"font-size:12px;\">" + p.descripcion + "</div>\n"
                    + "              </td>\n"
                    + "            </tr>\n"
                    + "          </table>\n"
                    + "          <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n"
                    + "          </table>\n"
                    + "          <table width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin-bottom:15px;\">\n"
                    + "            <tr>\n"
                    + "              <td align=\"left\" valign=\"middle\" style=\"padding:15px; font-family:Arial, Helvetica, sans-serif;\">\n"
                    + "              <div align=\"right\" style=\"font-size:25px;  color:#468847;\"> Comprado por </div> <div  align=\"right\" style=\"font-size:36px; color:#468847;\">" + monto + " U$S! </div>\n"
                    + "              </td>\n"
                    + "            </tr>\n" 
                    + "          </table>\n";
                    if (calificacion) {
                        cuerpo += 
                          "          <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-bottom:10px;\">\n"
                        + "            <tr>\n"
                        + "              <td align=\"left\" valign=\"middle\" style=\"padding:15px; background-color:#f89406 ; font-family:Arial, Helvetica, sans-serif;\"><div style=\"font-size:20px; color:#ffe77b;\">Calificaciones</div>\n"
                        + "                <div style=\"font-size:13px; color:#000;\">Te invitamos a calificar la compra en nuestra página web. <br>\n"
                        + "                  <br>\n"
                        + "                  <a href=\"" + linkCalif + "\" style=\"color:#000; text-decoration:underline;\">CLICK AQUI</a> PARA CALIFICAR LA COMPRA </div></td>\n"
                        + "            </tr>\n"
                        + "          </table>\n";
                    };
                   cuerpo += "     <table width=\"100%\" border=\"0\" style=\"background-color: burlywood;\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\n"
                    + "            <tr>\n"
                    + "              <td width=\"50%\" align=\"left\" valign=\"middle\" style=\"padding:10px;\"><table width=\"75%\" border=\"0\" cellspacing=\"0\" cellpadding=\"4\">\n"
                    + "                <tr>\n"
                    + "                  <td align=\"left\" valign=\"top\" style=\"font-family:Verdana, Geneva, sans-serif; font-size:14px; color:#000000;\"><b>Seguinos en</b></td>\n"
                    + "                </tr>\n"
                    + "                <tr>\n"
                    + "                  <td align=\"left\" valign=\"top\" style=\"font-family:Verdana, Geneva, sans-serif; font-size:12px; color:#000000;\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n"
                    + "                    <tr>\n"
                    + "                <td width=\"50%\" align=\"left\" valign=\"middle\" style=\"color:#564319; font-size:11px; font-family:Arial, Helvetica, sans-serif; padding:10px;\"><b>Facebook: </b> <a href=\"https://www.facebook.com\" style=\"color:#564319; text-decoration:none;\">Chebay FB</a><br>\n"
                    + "                <br>\n"
                    + "                 </tr>\n"
                    + "                  </table></td>\n"
                    + "                </tr>\n"
                    + "              </table></td>\n"
                    + "              <td width=\"50%\" align=\"left\" valign=\"middle\" style=\"color:#564319; font-size:11px; font-family:Arial, Helvetica, sans-serif; padding:10px;\"><b>Fecha:</b> " + fecha + "<br>\n"
                    + "                <b>Atencion al cliente: </b> <a href=\"mailto:chebaysend@gmail.com\" style=\"color:#564319; text-decoration:none;\">chebaysend@gmail.com</a><br>\n"
                    + "                <br>\n"
                    + "                <b>Pagina web: </b><br>\n"
                    + "Chebay: <a href=\"" + linkWeb + "\" target=\"_blank\"  style=\"color:#564319; text-decoration:none;\"> " + linkWeb + "</a></td>\n"
                    + "            </tr>\n"
                    + "          </table></td>\n"
                    + "      </tr>\n"
                    + "      </table>\n"
                    + "    <br>\n"
                    + "    <br></td>\n"
                    + "  </tr>\n"
                    + "</table>\n"
                    + "</body>\n"
                    + "</html>";

            return cuerpo;
        }

        public void EliminarCompra(long idCompra, string idTienda)
        {
            //idCompra esta asociada directamente uno a uno con la id del producto
            try
            {
                if (idCompra == 0)
                    throw new Exception("Debe pasar el identificador de una compra.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCompra = from cmp in context.compras
                                  where cmp.ProductoID == idCompra //solved
                                  select cmp;
                    context.compras.Remove(qCompra.FirstOrDefault());
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Compra ObtenerCompra(long idCompra, string idTienda)
        {
            //uno a uno con producto, obtener compra se relaciona con la id del mismo.
            try
            {
                if (idCompra == 0)
                    throw new Exception("Debe pasar el identificador de una compra.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCompra = from cmp in context.compras
                                  where cmp.ProductoID == idCompra //solved
                                  select cmp;
                    return qCompra.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<Compra> ObtenerCompras(string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCompra = from cmp in context.compras
                                  select cmp;
                    return qCompra.ToList();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public bool ExisteCompra(long idProducto, string idTienda)
        {
            try
            {
                if (idProducto == 0)
                    throw new Exception("Debe pasar el identificador de una compra.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qCompra = from cmp in context.compras
                                  where cmp.ProductoID == idProducto
                                  select cmp;
                    if (qCompra.Count() > 0)
                        return true;
                    return false;
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
        public void AgregarImagenProducto(ImagenProducto ip, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    context.imagenesproducto.Add(ip);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public List<ImagenProducto> ObtenerImagenProducto(long idProducto, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qImg = from img in context.imagenesproducto
                               where img.ProductoID == idProducto
                               select img;
                    List<ImagenProducto> ret = qImg.ToList();
                    return ret;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void EliminarImagenProducto(long idProducto, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qImg = from img in context.imagenesproducto
                               where img.ProductoID == idProducto
                               select img;
                    ImagenProducto ret = qImg.FirstOrDefault();
                    context.imagenesproducto.Remove(ret);
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

        #region atributos
        public void AgregarAtributo(List<Atributo> lAtributos, long idProducto, string idTienda)
        {
               try
               {
                   if (lAtributos == null)
                       throw new Exception("Tiene que pasar una lista de atributos.");
                   if (idProducto == 0)
                       throw new Exception("Tiene que pasar el identificador de un producto.");
                   chequearTienda(idTienda);
                   using (var context = ChebayDBContext.CreateTenant(idTienda))
                   {
                       foreach (Atributo a in lAtributos)
                       {
                           //Consulto si existe el atributo
                           var qAtributo = from atr in context.atributos
                                           where atr.AtributoID == a.AtributoID
                                           select atr;
                           
                           //Obtengo el producto.
                           var qProducto = from prd in context.productos
                                           where prd.ProductoID == idProducto
                                           select prd;
                           if (qProducto.Count() == 0)
                               throw new Exception("No existe el producto.");
                           Producto p = qProducto.FirstOrDefault();
                           
                           if (qAtributo.Count() > 0)
                           { //Modificar Atributo
                               Atributo at = qAtributo.FirstOrDefault();
                               at.etiqueta = a.etiqueta;
                               at.valor = a.valor;
                               if (at.productos == null)
                                   at.productos = new HashSet<Producto>();
                               at.productos.Add(p);
                               context.SaveChanges();
                           }
                           else
                           { //Agregar Atributo
                               if (a.productos == null)
                                   a.productos = new HashSet<Producto>();
                               a.productos.Add(p);
                               context.atributos.Add(a);
                               context.SaveChanges();
                           }
                       }
                   }
               }
               catch (Exception e)
               {
                   Debug.WriteLine(e.Message);
                   throw e;
               }
        }

        public void AgregarAtributo(Atributo a, long idProducto, string idTienda)
        {
            try
            {
                if (a == null)
                    throw new Exception("Tiene que pasar un atributo.");
                if (idProducto == 0)
                    throw new Exception("Tiene que pasar el identificador de un producto.");
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    //Consulto si existe el atributo
                    var qAtributo = from atr in context.atributos
                                    where atr.AtributoID == a.AtributoID
                                    select atr;

                    //Obtengo el producto.
                    var qProducto = from prd in context.productos
                                    where prd.ProductoID == idProducto
                                    select prd;
                    if (qProducto.Count() == 0)
                        throw new Exception("No existe el producto.");
                    Producto p = qProducto.FirstOrDefault();
                    
                    if (qAtributo.Count() > 0)
                    { //Modificar Atributo
                        Atributo at = qAtributo.FirstOrDefault();
                        at.etiqueta = a.etiqueta;
                        at.valor = a.valor;
                        if (at.productos == null)
                            at.productos = new HashSet<Producto>();
                        at.productos.Add(p);
                        context.SaveChanges();
                    }
                    else
                    { //Agregar Atributo
                        if (a.productos == null)
                            a.productos = new HashSet<Producto>();
                        a.productos.Add(p);
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

        public List<Atributo> ObtenerAtributos(long idProducto, string idTienda)
        {
            try
            {
                chequearTienda(idTienda);
                using (var context = ChebayDBContext.CreateTenant(idTienda))
                {
                    var qPrd = from prd in context.productos.Include("atributos")
                               where prd.ProductoID == idProducto
                               select prd;
                    Producto p = qPrd.FirstOrDefault();
                    return (List<Atributo>)p.atributos;
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
        #endregion
    }
}
