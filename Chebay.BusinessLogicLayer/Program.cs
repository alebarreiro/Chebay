using DataAccessLayer;
using Shared.Entities;
using System;
using Chebay.DataAccessLayerTests;
using Chebay.AlgorithmDLL;

namespace Chebay.BusinessLogicLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            //CustomTests ct = new CustomTests();
            //ct.CierreSubastaCompraDirecta();
            IChebayAlgorithm al = new ChebayAlgorithm();
            IDALSubasta sdal = new DALSubastaEF();
            IDALUsuario udal = new DALUsuarioEF();

            var listaprod = sdal.ObtenerTodosProductos("MobileCenter");
            var user = udal.ObtenerUsuarioFull("open_pirsaoz_user@tfbnw.net", "MobileCenter");
            var res = al.getProducts(listaprod,user);
            foreach (var r in res)
            {
                Console.WriteLine(r.nombre+r.ProductoID);
            }
            Console.Read();
        }
    }
}
