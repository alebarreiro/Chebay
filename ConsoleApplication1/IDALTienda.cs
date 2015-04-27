using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;


namespace DataAccessLayer
{
    interface IDALTienda
    {
        void AgregarAdministrador(Administrador a);

        //void ModificarAdministrador(Administrador a);

        Administrador ObtenerAdministrador(string idAdministrador);

        Administrador ObtenerAdministradorTienda(int idTienda);

        bool AutenticarAdministrador(string idAdministrador, string passwd);
        //Devuelve true si es el password correcto para el usuario.

        bool CambiarPassAdministrador(string idAdministrador, string passwdVieja, string passwdNueva);
        //Devuelve true si passwdVieja es la password del idAdministrador y pudo cambiarla por passwdNueva.

        void AgregarTienda(Tienda t);

        void ActualizarTienda(Tienda t);

        Tienda ObtenerTienda(int id);

        Tienda ObtenerTiendaURL(string url);

        Tienda ObtenerTiendaTitulo(string titulo);

        List<Tienda> ObtenerTodasTiendas();

        Tienda ObtenerTiendasAdministrador(string username);
    }
}
