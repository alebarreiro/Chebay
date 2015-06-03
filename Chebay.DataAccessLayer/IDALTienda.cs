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
        List<Tienda> ListarTiendas(string idAdmin);
        List<Tienda> ObtenerTodasTiendas();
        bool ExisteTienda(string idTienda);

        int ObtenerCantPaginas(string idAdmin);
        List<Tienda> ObtenerPagina(int n, string idAdmin);

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

        List<TipoAtributo> ListarTodosTipoAtributo(long idCategoria, string idTienda);

        //--PERSONALIZACIÓN DE LA TIENDA--
        void PersonalizarTienda(Personalizacion p, string idTienda);
        Personalizacion ObtenerPersonalizacionTienda(string idTienda);
        void EliminarPersonalizacion(string idTienda);


        bool AutenticarAdministrador(string idAdministrador, string passwd);
        
        //--ATRIBUTOS DE SESION ADMINISTRADOR--
        void AgregarAtributoSesion(AtributoSesion AtributoS);
        void EliminarAtributoSesion(string AdminID, string AtributoID);
        List<AtributoSesion> ObtenerAtributosSesion(string AdminID);

        //--REPORTES--
        DataReporte ObtenerReporte(string idTienda);

        //--ALGORITMO PERSONALIZACION--
        void ActualizarAlgoritmoPersonalizacion(Personalizacion pers);

    }
}