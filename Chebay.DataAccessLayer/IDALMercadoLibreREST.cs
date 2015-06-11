using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IDALMercadoLibreREST
    {
        List<DataCategoria> ListarCategoriasSitio(string sitio);
        List<DataCategoria> listarCategoriasHijas(string categoria);
        void ObtenerProductosMLporCategoria(string TiendaID, string limit, string categoryML, long categoryLocal);
    }
}
