using ChebayREST.Models;
using DataAccessLayer;
using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChebayREST.Controllers
{
    public class SubastaController : ApiController
    {
        public Subasta[] Get()
        {
            IDALSubasta ip = new DALSubastaEF();
            List<DataProducto> ldp = ip.ObtenerProductosPersonalizados("MobileCenter");
            //DataProducto dp = ldp.FirstOrDefault();
            Subasta[] ret = new Subasta[ldp.Count];
            int i = 0;
            foreach (DataProducto dp in ldp)
            {
                Subasta s = new Subasta
                {
                    Descripcion = dp.descripcion,
                    ProductoID = dp.ProductoID,
                    //fecha_cierre = dp.fecha_cierre,
                    IDOfertante = dp.idOfertante,
                    Nombre = dp.nombre,
                    PrecioActual = dp.precio_actual
                };
                ret[i] = s;
                i++;
            }
            return ret;
        }
        //CHEQUEAR
        public Subasta[] Get(string searchTerm)
        {
            IDALSubasta ip = new DALSubastaEF();
            List<DataProducto> ldp = ip.ObtenerProductosBuscados(searchTerm,"MobileCenter");
            //DataProducto dp = ldp.FirstOrDefault();
            Subasta[] ret = new Subasta[ldp.Count];
            int i = 0;
            foreach (DataProducto dp in ldp)
            {
                Subasta s = new Subasta
                {
                    Descripcion = dp.descripcion,
                    ProductoID = dp.ProductoID,
                    //fecha_cierre = dp.fecha_cierre,
                    IDOfertante = dp.idOfertante,
                    Nombre = dp.nombre,
                    PrecioActual = dp.precio_actual
                };
                ret[i] = s;
                i++;
            }
            return ret;
        }

    }
}
