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
        void EliminarAdministrador(string idAdmin);
        Administrador ObtenerAdministrador(string idAdmin);

        //--TIENDAS--
        void AgregarTienda(Tienda t, string idAdmin);
        void ActualizarTienda(Tienda t); //NO ACTUALIZAR LA URL.
        void EliminarTienda(string idTienda);
        Tienda ObtenerTienda(string idTienda);
        
        //--CATEGORIAS--
        void AgregarCategorias(List<Categoria> lCategorias, string idTienda);
        void AgregarCategoria(Categoria c, string idTienda);
        List<Categoria> ListarCategorias(string idTienda);
        Categoria ObtenerCategoria(string idTienda, long idCategoria);
        void EliminarCategoria(long idCategoria, string idTienda);
        void ModificarCategoria(Categoria c, string idTienda);

        //--TIPO ATRIBUTO--
        void AgregarTipoAtributo(TipoAtributo ta, long idCategoria, string idTienda);
        List<TipoAtributo> ListarTipoAtributo(string idTienda);
        List<TipoAtributo> ListarTipoAtributo(long idCategoria, string idTienda);
        void EliminarTipoAtributo(string idTipoAtributo, string idTienda);

        //--ATRIBUTOS--
        void AgregarAtributos(List<Atributo> lAtributos, string idTienda);
        void AgregarAtributo(Atributo a, string idTienda);
        List<Atributo> ObtenerAtributos(long idCategoria, string idTienda);
        Atributo ObtenerAtributo(long idAtributo, string idTienda);
        void EliminarAtributo(long idAtributo, string idTienda);
        void ModificarAtributo(Atributo a, string idTienda);
        
        //--PERSONALIZACIÓN DE LA TIENDA--
        void PersonalizarTienda(string color, string idTienda);

        bool AutenticarAdministrador(string idAdministrador, string passwd);
        
        void CambiarPassAdministrador(string idAdministrador, string passwdVieja, string passwdNueva);
        
        void AgregarAdminTienda(string idAdministrador, string idTienda);
        

        //--ATRIBUTOS DE SESION ADMINISTRADOR--
        void AgregarAtributoSesion(AtributoSesion AtributoS);
        void EliminarAtributoSesion(string AdminID, string AtributoID, string tienda);
        List<AtributoSesion> ObtenerAtributosSesion(string AdminID);
    }
}