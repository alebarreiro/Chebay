using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Shared.Entities;
using DataAccessLayer;
using System.Reflection;
using System.IO;

namespace Chebay.Algorithm
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        //default algorithm return the mosts visited product
        static List<Producto> default_recomendation_algorithm(List<Producto> products, Usuario user)
        {
            var query = from p in products
                        orderby (p.visitas.Count) descending
                        select p;
            return query.ToList();
        }


        static List<Producto> custom_algorithm(Tienda tienda, List<Producto> products, Usuario user)
        {
            string path = @"C:\Users\slave\Source\Repos\Chebay4\Chebay.AlgorithmDLL\bin\Debug\Chebay.AlgorithmDLL.dll";
            byte [] sticky = File.ReadAllBytes(path);
            Assembly ddl = Assembly.Load(sticky);
            //Assembly ddl = Assembly.LoadFile(path);
            var t = ddl.GetType("Chebay.AlgorithmDLL.ChebayAlgorithm");
            dynamic c = Activator.CreateInstance(t);
            List<Producto> res = (List<Producto>) c.getProducts(products, user);
            //var obj = Activator.CreateInstance(t);
            //var method = t.GetMethod("getProducts");
            //Object result = method.Invoke(obj, new Object[]{});
            


            return res;
        }
        
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            IDALUsuario udal = new DALUsuarioEF();
            IDALTienda tdal = new DALTiendaEF();
            IDALSubasta sdat = new DALSubastaEF();
            var productos = sdat.ObtenerTodosProductos("TestURL");
            var u = udal.ObtenerUsuario("alebarreiro@live.com", "TestURL");
            //var prods = custom_algorithm(productos, u);

            System.Console.Read();
            /*


            List<Tienda> tiendas = tdal.ObtenerTodasTiendas();

            foreach (var tienda in tiendas)
            {
                List<Producto> productos = sdat.ObtenerTodosProductos();
                //obtengo algoritmo
                Personalizacion p = tdal.ObtenerPersonalizacionTienda(tienda.TiendaID);
                List<Usuario> usuarios = udal.ObtenerTodosUsuariosFull(tienda.TiendaID);
                bool defaultalgorithm = false;
                if (p.algoritmo.Length == 0)
                {
                    defaultalgorithm = true;
                }
                foreach (var user in usuarios)
                {
                    List<Producto> rec;
                    if (defaultalgorithm)
                    {
                        rec = default_recomendation_algorithm(productos, user);
                    }
                    else
                    {
                        //rec = custom_algorithm(tienda, productos, user);
                    }
                    //savelist in mongo :)

                }
                

            }
            */
            //var host = new JobHost();
            
            

            // The following code ensures that the WebJob will be running continuously
            //host.RunAndBlock();
        }
    }
}
