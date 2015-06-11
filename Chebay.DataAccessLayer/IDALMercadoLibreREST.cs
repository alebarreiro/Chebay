using Shared.DataTypes;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IDALMercadoLibreREST
    {
        List<DataCategoria> ListarCategoriasSitio(string sitio);
        List<DataCategoria> listarCategoriasHijas(string categoria);
        void ObtenerProductosMLporCategoria(string TiendaID, string limit, string categoryML, long categoryLocal);
    }
}
