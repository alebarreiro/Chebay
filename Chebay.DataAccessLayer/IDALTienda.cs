using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using Shared.DataTypes;


namespace DataAccessLayer
{
    public interface IDALTienda
    {
        //--ADMINISTRADOR--
        void AgregarAdministrador(Administrador a);
        void EliminarAdministrador(string idAdministrador);
        Administrador ObtenerAdministrador(string idAdministrador);

        //--TIENDAS--
        void AgregarTienda(Tienda t, string idAdmin);
        void ActualizarTienda(Tienda t); //NO ACTUALIZAR LA URL.
        void EliminarTienda(string idTienda);
        Tienda ObtenerTienda(string idTienda);
        
        //--CATEGORIAS--
        void AgregarCategorias(List<Categoria> lCategorias, string urlTienda); //idPadre no puede ser null.
        void AgregarCategoria(Categoria c, string urltienda);
        List<Categoria> ListarCategorias(string idTienda);
        Categoria ObtenerCategoria(string idTienda, long idCategoria);


        //CU: 1.1 INICIAR SESIÓN BACK OFFICE
        bool AutenticarAdministrador(string idAdministrador, string passwd);
        //Devuelve true si es el password correcto para el usuario.
        
        void CambiarPassAdministrador(string idAdministrador, string passwdVieja, string passwdNueva);
        //Devuelve true si passwdVieja es la password del idAdministrador y pudo cambiarla por passwdNueva.

        void AgregarAdminTienda(string idAdministrador, string idTienda);
        //Agrega a idAdministrador a idTienda.

        //CU: 1.4 INGRESAR TIPO DE ATRIBUTO Y 1.5 ALTA ATRIBUTO
        void AgregarAtributos(List<Atributo> lAtributos, string urlTienda);
        //FALTA IMPLEMENTAR.

        
        
        

    }
}