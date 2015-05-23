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
using Shared.DataTypes;

namespace Chebay.Algorithm
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        
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
                    defaultalgorithm = true;
                }
                foreach (var user in usuarios)
                {
                    System.Console.WriteLine("USUARIO::"+user.UsuarioID);
                    if (defaultalgorithm)
                    {
                        Algorithms def = new Algorithms();
                        Thread defThread = new Thread(() =>
                        {
                            def.default_recomendation_algorithm(productos, user, tienda.TiendaID);
                        });
                        defThread.Start();
                    }
                    else
                    {
                        try
                        {
                            Thread t = new Thread(
                                delegate(){
                                    System.Console.WriteLine("CustomAlgorithm");
                                    Algorithms a = new Algorithms();
                                    a.custom_algorithm(pers, productos, user, tienda.TiendaID);
                                });
                            t.Start();
                         
                        }
                        catch (Exception E)
                        {
                            System.Console.WriteLine("Error ejecutar algoritmo custom... Ejecutando por defecto...");
                            Thread t = new Thread(
                                delegate(){
                                    System.Console.WriteLine("DefaultAlgorithm");
                                    Algorithms a = new Algorithms();
                                    a.default_recomendation_algorithm(productos, user,tienda.TiendaID);
                                });
                            t.Start();
                                
                        }
                    }
                }
            }
            System.Console.Read();
            //var host = new JobHost();
            
            

            // The following code ensures that the WebJob will be running continuously
            //host.RunAndBlock();
        }
    }
}
