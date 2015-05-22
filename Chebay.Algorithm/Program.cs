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


        static List<Producto> custom_algorithm(Personalizacion personalizacion, List<Producto> products, Usuario user)
        {
            //string path = @"C:\Users\slave\Source\Repos\Chebay4\Chebay.AlgorithmDLL\bin\Debug\Chebay.AlgorithmDLL.dll";
            //byte [] sticky = File.ReadAllBytes(path);
            Assembly ddl = Assembly.Load(personalizacion.algoritmo);
            var t = ddl.GetType("Chebay.AlgorithmDLL.ChebayAlgorithm");
            dynamic c = Activator.CreateInstance(t);
            List<Producto> res = (List<Producto>) c.getProducts(products, user);
            return res;
        }
        
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            IDALUsuario udal = new DALUsuarioEF();
            IDALTienda tdal = new DALTiendaEF();
            IDALSubasta sdat = new DALSubastaEF();
            //var productos = sdat.ObtenerTodosProductos("TestURL");
            //var u = udal.ObtenerUsuario("alebarreiro@live.com", "TestURL");
            //Tienda t = tdal.ObtenerTienda("TestURL");
            //try
            //{
            //    var prods = custom_algorithm(t, productos, u);

            //}
            //catch (Exception E)
            //{
            //    System.Console.WriteLine("Error ejecutar DLL...");
            //}

            System.Console.Read();
            


            List<Tienda> tiendas = tdal.ObtenerTodasTiendas();

            foreach (var tienda in tiendas)
            {
                System.Console.WriteLine(tienda.TiendaID);

                List<Producto> productos = sdat.ObtenerTodosProductos(tienda.TiendaID);
                //obtengo algoritmo
                Personalizacion pers = tdal.ObtenerPersonalizacionTienda(tienda.TiendaID);
                List<Usuario> usuarios = udal.ObtenerTodosUsuariosFull(tienda.TiendaID);
                bool defaultalgorithm = false;


                if (pers.algoritmo== null || pers.algoritmo.Length == 0)
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
                        try
                        {
                            rec = custom_algorithm(pers, productos, user);
                        }
                        catch (Exception E)
                        {
                            System.Console.WriteLine("Error ejecutar algoritmo custom... Ejecutando por defecto...");
                            rec = default_recomendation_algorithm(productos, user);
                        }
                    }
                    //savelist in mongo :)
                    foreach (var pr in rec)
                    {
                        System.Console.WriteLine(+pr.ProductoID + pr.nombre);
                    }

                }
                
            }
            
            //var host = new JobHost();
            
            

            // The following code ensures that the WebJob will be running continuously
            //host.RunAndBlock();
        }
    }
}
