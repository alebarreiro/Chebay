using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using Shared.DataTypes;

namespace DataAccessLayer
{
    public interface IDALSubasta
    {

        //--PRODUCTO--
        void AgregarProducto(Producto p, string idTienda);
        Producto ObtenerProducto(long idProducto, string idTienda);
        List<Producto> ObtenerTodosProductos(string idTienda); //algorithm
        
        List<DataProducto> ObtenerProductosPersonalizados(string urlTienda); //Devuelve los últimos 10 productos publicados para el index.
        List<DataProducto> ObtenerProductosPorTerminar(int cantProductos, string urlTienda); //Devuelve los 10 productos más cercanos a su fecha de cierre.
        List<DataProducto> ObtenerProductosBuscados(string searchTerm, string urlTienda);

        List<Producto> ObtenerProductosVencidos(DateTime ini, DateTime fin, string idTienda); //FALTA IMPLEMENTAR
        //Devuelve los productos que se vencieron entre ini y fin.

        List<Producto> ObtenerProductosCategoria(long idCategoria, string idTienda);
        
        Producto ObtenerInfoProducto(long idProducto, string idTienda, string idUsuario);

        //--ATRIBUTOS--
        void AgregarAtributo(Atributo a, string idTienda);
        void AgregarAtributo(List<Atributo> la, string idTienda);
        List<Atributo> ObtenerAtributos(long idProducto, string idTienda);
        Atributo ObtenerAtributo(long idAtributo, string idTienda);
        void EliminarAtributo(long idAtributo, string idTienda);

        //--COMENTARIO--
        void AgregarComentario(Comentario c, string idTienda);
        Comentario ObtenerComentario(long idComentario, string idTienda);
        List<Comentario> ObtenerComentarios(long idProducto, string idTienda);
        List<Comentario> ObtenerComentarios(string idTienda);
        void EliminarComentario(long idComentario, string idTienda);

        //--FAVORITOS--
        void AgregarFavorito(long idProducto, string idUsuario, string idTienda);
        bool EsFavorito(long idProducto, string idUsuario, string idTienda);
        int ObtenerCantFavoritos(long idProducto, string idTienda);
        void EliminarFavorito(long idProducto, string idUsuario, string idTienda);
        
        //--COMPRAS--
        void AgregarCompra(Compra c, string idTienda);
        Compra ObtenerCompra(long idCompra, string idTienda);
        List<Compra> ObtenerCompras(string idTienda);
        void EliminarCompra(long idCompra, string idTienda);

        //--OFERTAS--
        void OfertarProducto(Oferta o, string idTienda);
        List<Oferta> ObtenerOfertas(int n, long idProducto, string idTienda);
        List<Oferta> ObtenerOfertas(string idTienda);
        Oferta ObtenerOferta(long idOferta, string idTienda);
        void EliminarOferta(long idOferta, string idTienda);
        Oferta ObtenerOfertaGanadora(long idProducto, string idTienda); //FALTA IMPLEMENTAR.

        //--IMAGENES--
        void AgregarImagenProducto(ImagenProducto ip, string idTienda);
        List<ImagenProducto> ObtenerImagenProducto(long idProducto, string idTienda);
        void EliminarImagenProducto(long idProducto, string idTienda);


        //void AgregarImagenProducto(Imagen img, Producto p);
        /*
        List<Producto> ObtenerProductosVisitados(string idUsuario);
        List<Producto> ObtenerProductosFavoritos(string idUsuario);
        List<Producto> ObtenerProductosComprados(string idUsuario);
        List<Producto> ObtenerProductosOfertados(string idUsuario);
        List<Producto> ObtenerProductosPublicados(string idUsuario);
        */
        
        

    }
}
