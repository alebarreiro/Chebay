using DataAccessLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chebay.BusinessLogicLayer
{
    public class BLPersonalizacion
    {
        public void PersonalizarTienda(string colorPrimario, string colorSecundario, int estilo, string idTienda)
        {
            IDALTienda it = new DALTiendaEF();
            
            //Obtener Template
            Personalizacion p = it.ObtenerPersonalizacionTienda(idTienda);

            //Modificar Template
            string css = p.datos;
            css.Replace("#e44d26",colorPrimario);
            css.Replace("#f16529", colorSecundario);

            //Guardar CSS en la base
            p.datos = css;
            it.PersonalizarTienda(p, idTienda);

        }
    }
}
