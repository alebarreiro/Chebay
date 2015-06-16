using DataAccessLayer;
using Shared.Entities;
using System;
using Chebay.DataAccessLayerTests;
using Chebay.AlgorithmDLL;
using Shared.DataTypes;

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
            DataRecomendacion dr = new DataRecomendacion{ UsuarioID = "alebarreiro@live .com"};
            

            var prod = udal.ObtenerRecomendacionesUsuario("MobileCenter",dr);
            if (prod == null)
                Console.WriteLine("ES NULL");
            else
            {
                foreach (var i in prod.productos)
                {
                    Console.WriteLine(i.nombre);
                }
            }
            
            //var listaprod = sdal.ObtenerTodosProductos("MobileCenter");
            //var user = udal.ObtenerUsuarioFull("open_pirsaoz_user@tfbnw.net", "MobileCenter");
            //var res = al.getProducts(listaprod,user);
            //foreach (var r in res)
            //{
            //    Console.WriteLine(r.nombre+r.ProductoID);
            //}
            Console.Read();
        }
    }
}
