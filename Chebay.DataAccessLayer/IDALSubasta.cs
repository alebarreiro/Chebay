﻿using System;
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
        List<DataProducto> ObtenerProductosPorTerminar(int cantProductos, string urlTienda); //Devuelve los 10 productos más cercanos a su fecha de cierre.

        List<Producto> ObtenerProductosCategoria(long idCategoria, string idTienda);//FALTA IMPLEMENTAR
        
        Producto ObtenerInfoProducto(long idProducto, string idTienda, string idUsuario);

        void OfertarProducto(Oferta o, string idTienda);

        //--ATRIBUTOS--
        void AgregarAtributos(List<Atributo> lAtributos, string idTienda);
        void AgregarAtributo(Atributo a, string idTienda);
        List<Atributo> ObtenerAtributos(long idCategoria, string idTienda);
        Atributo ObtenerAtributo(long idAtributo, string idTienda);
        void EliminarAtributo(long idAtributo, string idTienda);
        void ModificarAtributo(Atributo a, string idTienda);

        //--COMENTARIO--
        void AgregarComentario(Comentario c, string idTienda);

        //--FAVORITOS--
        void AgregarFavorito(long idProducto, string idUsuario, string idTienda);
        void EliminarFavorito(long idProducto, string idUsuario, string idTienda);
        



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
