using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;


namespace DataAccessLayer
{
    public interface IDALUsuario
    {
        void AgregarUsuario(Usuario u, string idTienda);
        void ActualizarUsuario(Usuario u, string idTienda);
        Usuario ObtenerUsuario(string idUsuario, string idTienda);
        List<Usuario> ObtenerTodosUsuarios(string idTienda);
        void EliminarUsuario(string idUsuario, string idTienda);

        //Falta completar
        void AgregarVisita(string idUsuario, string idProducto);
        void AgregarFavorito(string idUsuario);



        
                
    }
}