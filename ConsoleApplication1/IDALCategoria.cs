using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace DataAccessLayer
{
    public interface IDALCategoria
    {
        void AgregarCategoria(Categoria c);

        void ActualizarCategoria(Categoria c);

        void BorrarCategoria(Categoria c);

        List<Categoria> ObtenerTodasCategorias(string nombreTienda);

        Categoria ObtenerCategoria(string nombreTienda, string nombre);

       
    }
}
