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
        List<DataProducto> ObtenerProductosPersonalizados(string urlTienda); //Devuelve los últimos 10 productos publicados para el index.

        void OfertarProducto(Oferta o, string idTienda);

        List<DataProducto> ObtenerProductosPorTerminar(string urlTienda); //Devuelve los 10 productos más cercanos a su fecha de cierre.

        //void AgregarImagenProducto(Imagen img, Producto p);
        /*
        List<Producto> ObtenerProductosCategoria(string nombreCategoria);
        List<Producto> ObtenerProductosVisitados(string idUsuario);
        List<Producto> ObtenerProductosFavoritos(string idUsuario);
        List<Producto> ObtenerProductosComprados(string idUsuario);
        List<Producto> ObtenerProductosOfertados(string idUsuario);
        List<Producto> ObtenerProductosPublicados(string idUsuario);
        */
        void AgregarComentario(Comentario c, string idTienda);
        

    }
}
