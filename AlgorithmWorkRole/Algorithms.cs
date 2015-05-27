using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.Entities;
using DataAccessLayer;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

namespace AlgorithmWorkRole
{
    public class Algorithms
    {


        //default algorithm return the mosts visited product
        public void default_recomendation_algorithm(List<Producto> products, Usuario user, String tiendaID)
        {
            IDALUsuario udal = new DALUsuarioEF();

            var query = from p in products
                        orderby (p.visitas.Count) descending
                        select p;
            DataRecomendacion dr = new DataRecomendacion { UsuarioID = user.UsuarioID, productos = new List<DataProducto>() };
            foreach (var p in query.ToList())
            {
                dr.productos.Add(new DataProducto(p));
            }
            udal.AgregarRecomendacionesUsuario(tiendaID, dr);

        }


        public void custom_algorithm(Personalizacion personalizacion, List<Producto> products, Usuario user, string tiendaID)
        {

            Assembly ddl = Assembly.Load(personalizacion.algoritmo);
            var t = ddl.GetType("Chebay.AlgorithmDLL.ChebayAlgorithm");
            dynamic c = Activator.CreateInstance(t);
            DataRecomendacion dr = new DataRecomendacion { UsuarioID = user.UsuarioID };
            Thread timeThread = new Thread(() =>
            {
                try
                {
                    dr.productos = (List<DataProducto>)c.getProducts(products, user);
                    IDALUsuario udal = new DALUsuarioEF();
                    udal.AgregarRecomendacionesUsuario(tiendaID, dr);
                }
                catch (Exception e)
                {
                    //si falla entonces default
                    default_recomendation_algorithm(products, user, tiendaID);
                }

            });
            timeThread.Start();

            bool finished = timeThread.Join(5000);
            if (!finished)
            {
                //Debug.WriteLine("Probablemente un loop infinito...");
                timeThread.Abort();
                //suspendo y llamo default
                default_recomendation_algorithm(products,user,tiendaID);
            }
                    
        }
    }
}
