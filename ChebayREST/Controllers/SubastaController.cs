using ChebayREST.Models;
using DataAccessLayer;
using Shared.DataTypes;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChebayREST.Controllers
{
    public class SubastaController : ApiController
    {

        //URL: http://chebayrest1930.azurewebsites.net/api/subasta
        public Subasta[] Get()
        {
            IDALSubasta ip = new DALSubastaEF();
            List<DataProducto> ldp = ip.ObtenerProductosPersonalizados("MobileCenter");
            //DataProducto dp = ldp.FirstOrDefault();
            Subasta[] ret = new Subasta[ldp.Count];
            int i = 0;
            foreach (DataProducto dp in ldp)
            {
                Debug.WriteLine("GET");
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

        //URL: api/subasta?searchTerm=terminosdebusqueda;
        public Subasta[] Get(string searchTerm)
        {
            IDALSubasta ip = new DALSubastaEF();
            List<DataProducto> ldp = ip.ObtenerProductosBuscados(searchTerm,"MobileCenter");
            Subasta[] ret = new Subasta[ldp.Count];
            int i = 0;
            Debug.WriteLine("A");
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
                Debug.WriteLine("b");
            }
            return ret;
        }

        //URL: api/subasta?monto=xx&idProducto=xx&idUsuario=xx&idTienda=xx;
        public void Get(int monto, long idProducto, string idUsuario)
        {
            try
            { 
                IDALSubasta ip = new DALSubastaEF();
                Oferta o = new Oferta
                {
                    monto = monto,
                    ProductoID = idProducto,
                    UsuarioID = idUsuario
                };
                ip.OfertarProducto(o, "MobileCenter");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
