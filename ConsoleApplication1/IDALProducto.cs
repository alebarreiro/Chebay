using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace DataAccessLayer
{
    public interface IDALProducto
    {
        void AgregarProducto(Producto p);

        void ModificarProducto(Producto p);

        //void AgregarImagenProducto(Imagen img, Producto p);

        //void AgregarComentarioProducto(Comentario, Producto p);

        Producto ObtenerProducto(int id);

        List<Producto> ObtenerProductosCategoria(string nombreTienda, string nombreCategoria);

        List<Producto> ObtenerProductosVisitados(int idUsuario);

        List<Producto> ObtenerProductosFavoritos(int idUsuario);

        List<Producto> ObtenerProductosComprados(int idUsuario);

        List<Producto> ObtenerProductosOfertados(int idUsuario);

        List<Producto> ObtenerProductosPublicados(int idUsuario);

    }
}
