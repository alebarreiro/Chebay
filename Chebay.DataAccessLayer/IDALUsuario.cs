using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using Shared.DataTypes;


namespace DataAccessLayer
{
    public interface IDALUsuario
    {
        //--USUARIOS--
        void AgregarUsuario(Usuario u, string idTienda);
        void ActualizarUsuario(Usuario u, string idTienda);
        Usuario ObtenerUsuario(string idUsuario, string idTienda);
        List<Usuario> ObtenerTodosUsuarios(string idTienda);
        List<Usuario> ObtenerTodosUsuariosFull(string idTienda); //algorithm
        Usuario ObtenerUsuarioFull(string idUsuario, string idTienda);
        void EliminarUsuario(string idUsuario, string idTienda);

        //--CALIFICACIONES--
        void AgregarCalificacion(Calificacion c, string idTienda);
        Calificacion ObtenerCalificacion(long idCalificacion, string idTienda);
        List<Calificacion> ObtenerCalificaciones(string idTienda);
        void EliminarCalificacion(long idCalificacion, string idTienda);

        DataCalificacion ObtenerCalificacionUsuario(string idUsuario, string idTienda);
        DataPuedoCalificar PuedoCalificar(long idProducto, string idUsuario, string idTienda);

        //--IMAGENES--
        void AgregarImagenUsuario(ImagenUsuario iu, string idTienda);
        ImagenUsuario ObtenerImagenUsuario(string idUsuario, string idTienda);
        void EliminarImagenUsuario(string idUsuario, string idTienda);

        //--RECOMENDACIONES--
        DataRecomendacion ObtenerRecomendacionesUsuario(string TiendaID, DataRecomendacion dataRecomendacion);
        Task AgregarRecomendacionesUsuario(string TiendaID, DataRecomendacion dataRecomendacion);
        Task InicializarColeccionRecomendaciones(string TiendaID);

        List<DataFactura> ObtenerFactura(string idUsuario, string idTienda);

    }
}