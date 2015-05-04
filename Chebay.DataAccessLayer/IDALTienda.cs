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

        //--ATRIBUTOS--
        void AgregarAtributos(List<Atributo> lAtributos, string urlTienda);
        void AgregarAtributo(Atributo a, string urlTienda);
        List<Atributo> ObtenerAtributos(long idCategoria, string idTienda);
        
        bool AutenticarAdministrador(string idAdministrador, string passwd);
        
        void CambiarPassAdministrador(string idAdministrador, string passwdVieja, string passwdNueva);
        
        void AgregarAdminTienda(string idAdministrador, string idTienda);
        
    }
}