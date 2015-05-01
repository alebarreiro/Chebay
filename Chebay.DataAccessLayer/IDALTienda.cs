using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;


namespace DataAccessLayer
{
    public interface IDALTienda
    {
        void AgregarAdministrador(string idAdmin, string pass);
        //Agrega el Administrador idAdmin con password pass a la base de datos.
        //TESTEADA.

    //CU: 1.1 INICIAR SESIÓN BACK OFFICE
        bool AutenticarAdministrador(string idAdministrador, string passwd);
        //Devuelve true si es el password correcto para el usuario.
        //TESTEADA.
        
        void CambiarPassAdministrador(string idAdministrador, string passwdVieja, string passwdNueva);
        //Devuelve true si passwdVieja es la password del idAdministrador y pudo cambiarla por passwdNueva.

        // -- TIENDAS --

    //En la Parte 1 del BackOffice
        void AgregarTienda(string nom, string desc, string url, string idAdmin);
        //Completa el nombre, descripción, una URL (TiendaID).
        //TESTEADA.

        void ActualizarTienda(string nomNuevo, string descNueva, string urlVieja);
        //Cambiar nombre o descripción de t.
        //TESTEADA.

        void CambiarURLTienda(string idTienda, string nuevaURL);
        //Cambia la URL de idTienda por nuevaURL.

        void AgregarAdminTienda(string idAdministrador, string idTienda);
        //Agrega a idAdministrador a idTienda.
        
        // -- CATEGORIAS --

    //CU: 1.2 INGRESAR CATEGORIA Y 1.3 ALTA CATEGORIA
        void AgregarCategoriaCompuesta(string idCategoria, string idPadre, string urlTienda);
        //idPadre no puede ser null. La categoría raiz se crea cuando se crea la tienda.

        void AgregarCategoriaSimple(string idCategoria, long idPadre);
        //idPadre no puede ser null.

    //CU: Alta Subasta
        List<Categoria> ListarCategorias(string idTienda);
        //Lista las Categorias de idTienda.

    //CU: 1.4 INGRESAR TIPO DE ATRIBUTO Y 1.5 ALTA ATRIBUTO
        void AgregarAtributo(string urltienda, string idCategoria, string idAtributo, string valor);

        void EliminarAdministrador(string idAdministrador);
        Administrador ObtenerAdministrador(string idAdministrador);
        void EliminarTienda(string idTienda);
        Tienda ObtenerTienda(string idTienda);

    }
}
