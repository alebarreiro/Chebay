using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Shared.Entities;
using DataAccessLayer;

namespace Chebay.Algorithm
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        //default algorithm return the mosts visited product
        static List<Producto> recomendation_algorithm(List<Producto> products, Usuario user)
        {
            var query = from p in products
                        orderby (p.visitas.Count) descending
                        select p;
            return query.ToList();
        }
        
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {

            IDALUsuario udal = new DALUsuarioEF();
            IDALTienda tdal = new DALTiendaEF();
            IDALSubasta sdat = new DALSubastaEF();

            List<Tienda> tiendas = tdal.ObtenerTodasTiendas();

            foreach (var t in tiendas)
            {
                List<Producto> productos = sdat.ObtenerTodosProductos();
                //obtengo algoritmo
                Personalizacion p = tdal.ObtenerPersonalizacionTienda(t.TiendaID);
                List<Usuario> usuarios = udal.ObtenerTodosUsuariosFull(t.TiendaID);
                bool defaultalgorithm = false;
                if (p.algoritmo.Length == 0)
                {
                    defaultalgorithm = true;
                }
                foreach (var u in usuarios)
                {
                    List<Producto> rec;
                    if (defaultalgorithm)
                    {
                        rec = recomendation_algorithm(productos, u);
                    }
                    else
                    {
                        //rec = reflection_algorithm(dll, productos, u);
                    }
                    //savelist in mongo :)

                }
                

            }
            var host = new JobHost();
            
            

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
