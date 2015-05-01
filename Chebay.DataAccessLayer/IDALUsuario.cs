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
        void AgregarUsuario(Usuario u);
        void ActualizarUsuario(Usuario u);
        Usuario ObtenerUsuario(string id);
        List<Usuario> ObtenerTodosUsuarios();
        //Falta completar
        void AgregarVisita(string idUsuario, string idProducto);
        void AgregarFavorito(string idUsuario);



        
                
    }
}