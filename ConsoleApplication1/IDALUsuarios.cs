using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;


namespace DataAccessLayer
{
    public interface IDALUsuarios
    {
        void AgregarUsuario(Usuario u);

        void ActualizarUsuario(Usuario u);

        Usuario ObtenerUsuario(int id);
        //Obtener Usuario por ID.

        List<Usuario> ObtenerTodosUsuarios();

        Usuario ObtenerUsuarioMail(string email);
        //Obtener Usuario por Mail.
    }
}