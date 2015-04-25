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
        void AgregarTienda(Tienda t);

        void ActualizarTienda(Tienda t);

        Tienda ObtenerTienda(int id);

        Tienda ObtenerTiendaURL(string url);

        Tienda ObtenerTiendaTitulo(string titulo);

        List<Tienda> ObtenerTodasTiendas();

        Tienda ObtenerTiendasAdministrador(string username);

    }
}
