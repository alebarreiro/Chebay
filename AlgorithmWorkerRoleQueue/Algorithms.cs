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

namespace AlgorithmWorkerRoleQueue
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


        public void Run(int instance, int threads)
        {
            System.Console.WriteLine("New thread " + instance);
            Debug.WriteLine("New thread " + instance);

            IDALUsuario udal = new DALUsuarioEF();
            IDALTienda tdal = new DALTiendaEF();
            IDALSubasta sdat = new DALSubastaEF();

            List<Tienda> tiendas = tdal.ObtenerTodasTiendas();

            foreach (var tienda in tiendas)
            {
                //hardocoded numero instancias.

                //tiene asignado algunas instancias de la tienda.
                if (Math.Abs(tienda.TiendaID.GetHashCode() % threads) == instance)
                {
                    System.Console.WriteLine("Instance::" + instance + "::" + tienda.TiendaID);
                    System.Console.WriteLine("TiendaHash: " + tienda.TiendaID.GetHashCode());
                    Debug.WriteLine("Instance::" + instance + "::" + tienda.TiendaID);
                    Debug.WriteLine("TiendaHash: " + tienda.TiendaID.GetHashCode());

                    List<Producto> productos = sdat.ObtenerTodosProductos(tienda.TiendaID);
                    //obtengo algoritmo
                    Personalizacion pers = tdal.ObtenerPersonalizacionTienda(tienda.TiendaID);
                    List<Usuario> usuarios = udal.ObtenerTodosUsuariosFull(tienda.TiendaID);
                    bool defaultalgorithm = false;

                    if (pers.algoritmo == null || pers.algoritmo.Length == 0)
                    {
                        defaultalgorithm = true;
                    }


                    //creo indice
                    Task index = udal.InicializarColeccionRecomendaciones(tienda.TiendaID);
                    index.Wait();

                    foreach (var user in usuarios)
                    {
                        //Debug.WriteLine("USUARIO::" + user.UsuarioID);
                        if (defaultalgorithm)
                        {
                            Algorithms def = new Algorithms();
                            //Thread defThread = new Thread(() =>
                            //{
                            def.default_recomendation_algorithm(productos, user, tienda.TiendaID);
                            //});
                            //defThread.Start();
                        }
                        else
                        {
                            //Thread t = new Thread(
                            //delegate()
                            //{
                            //Debug.WriteLine("CustomAlgorithm");
                            Algorithms a = new Algorithms();
                            a.custom_algorithm(pers, productos, user, tienda.TiendaID);
                            //  });
                            //t.Start();

                        }
                    }//IF tienda es de instancia...
                }
            }



        }




    }
}
