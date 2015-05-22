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
using System.Threading;
using System.Diagnostics;

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
            Assembly ddl = Assembly.Load(personalizacion.algoritmo);
            var t = ddl.GetType("Chebay.AlgorithmDLL.ChebayAlgorithm");
            dynamic c = Activator.CreateInstance(t);
            List<Producto> res = null;
            bool finish = false;

            Thread thread = new Thread(delegate()
            {
                res = (List<Producto>)c.getProducts(products, user);
                finish = true;
            });

            int timeout = 50;
            while (!finish)
            {
                Thread.Sleep(100);
                if(!finish)
                    timeout--;

                if (timeout == 0)
                {
                    Debug.WriteLine("Timeout algorithm, infinite loop(?)");
                    finish = true;
                    thread.Abort();
                    //return default algorithm
                    res = default_recomendation_algorithm(products, user);
                }
            }
            return res;
        }
        
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            IDALUsuario udal = new DALUsuarioEF();
            IDALTienda tdal = new DALTiendaEF();
            IDALSubasta sdat = new DALSubastaEF();

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
                    System.Console.WriteLine(tienda.TiendaID + "default algorithm");
                    defaultalgorithm = true;
                }
                foreach (var user in usuarios)
                {
                    System.Console.WriteLine("USUARIO::"+user.UsuarioID);
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

                //System.Console.Read();
            }
            
            //var host = new JobHost();
            
            

            // The following code ensures that the WebJob will be running continuously
            //host.RunAndBlock();
        }
    }
}
