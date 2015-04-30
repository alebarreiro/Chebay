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
        void AgregarProducto(string idUsuario, string nomProducto, string descProducto, int precioBase, int precioCompra, DateTime fechaCierre, string idCategoria);
        //Crea una subasta.

        List<DataProducto> ObtenerProductosPersonalizados();
        //Devuelve los últimos 10 productos publicados para el index.

        //DataProductoFull ObtenerProducto(long idProducto);
        //Devuelve toda la info del producto idProducto.

        void OfertarProducto(string idOfertante, long idProducto, double monto);

        //void AgregarImagenProducto(Imagen img, Producto p);

        List<Producto> ObtenerProductosCategoria(string nombreCategoria);

        List<Producto> ObtenerProductosVisitados(string idUsuario);

        List<Producto> ObtenerProductosFavoritos(string idUsuario);

        List<Producto> ObtenerProductosComprados(string idUsuario);

        List<Producto> ObtenerProductosOfertados(string idUsuario);

        List<Producto> ObtenerProductosPublicados(string idUsuario);

        void AgregarComentario(string texto, long idProducto, string idUsuario);
        

    }
}
