﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;


namespace DataAccessLayer
{
    public interface IDALUsuario
    {
        //--USUARIOS--
        void AgregarUsuario(Usuario u, string idTienda);
        void ActualizarUsuario(Usuario u, string idTienda);
        Usuario ObtenerUsuario(string idUsuario, string idTienda);
        List<Usuario> ObtenerTodosUsuarios(string idTienda);
        void EliminarUsuario(string idUsuario, string idTienda);

        //--CALIFICACIONES--
        void AgregarCalificacion(Calificacion c, string idTienda);
        Calificacion ObtenerCalificacion(long idCalificacion, string idTienda);
        List<Calificacion> ObtenerCalificaciones(string idTienda);
        void EliminarCalificacion(long idCalificacion, string idTienda);

        double ObtenerCalificacionUsuario(string idUsuario, string idTienda);



        
                
    }
}