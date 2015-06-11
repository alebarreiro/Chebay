
using DataAccessLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChebayREST.Controllers
{
    public class TiendaController : ApiController
    {

        public String[] Get()
        {
            IDALTienda it = new DALTiendaEF();
            List<Tienda> lt = it.ObtenerTodasTiendas();
            String[] ret = new String[lt.Count];
            int i = 0;
            foreach (Tienda t in lt)
            {
                ret[i] = t.TiendaID;
                i++;
            }
            return ret;
        }
    }
}
