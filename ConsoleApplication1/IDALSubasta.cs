using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace DataAccessLayer
{
    public interface IDALSubasta
    {
        void AgregarProducto(Producto p);

        void ModificarProducto(Producto p);

        //void AgregarImagenProducto(Imagen img, Producto p);

        Producto ObtenerProducto(long idProducto, string idUsuario);

        List<Producto> ObtenerProductosCategoria(string nombreTienda, string nombreCategoria);

        List<Producto> ObtenerProductosVisitados(string idUsuario);

        List<Producto> ObtenerProductosFavoritos(string idUsuario);

        List<Producto> ObtenerProductosComprados(string idUsuario);

        List<Producto> ObtenerProductosOfertados(string idUsuario);

        List<Producto> ObtenerProductosPublicados(string idUsuario);

        void AgregarComentario(string texto, long idProducto, string idUsuario);
        

    }
}
