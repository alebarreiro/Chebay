using DataAccessLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chebay.BusinessLogicLayer
{
    public class BLPersonalizacion
    {
        public void PersonalizarTienda(string colorPrimario, string colorSecundario, int estilo, byte[] img, string idTienda)
        {
            IDALTienda it = new DALTiendaEF();
            
            //Obtener Template
            Personalizacion p ;
            if (estilo == 1)
                p = it.ObtenerPersonalizacionTienda("template1");
            else // if (estilo == 2)
                p = it.ObtenerPersonalizacionTienda("template2");

            //Modificar Template
            string newcss = p.css;
            Debug.WriteLine(newcss);
            newcss = newcss.Replace("colorPrimario", colorPrimario);
            newcss = newcss.Replace("colorSecundario", colorSecundario);
            Debug.WriteLine(newcss);

            //Guardar CSS en la base
            p.css = newcss;
            p.template = estilo;
            p.backgroud_image = img;
            p.PersonalizacionID = idTienda;//
            it.PersonalizarTienda(p, idTienda);

        }
    }
}
