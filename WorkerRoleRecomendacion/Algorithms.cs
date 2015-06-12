using DataAccessLayer;
using Shared.DataTypes;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerRoleRecomendacion
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
                Debug.WriteLine("ALGORITMO PERSONALIZADO SUSPENDIDO... EXCESO TIEMPO");
                timeThread.Abort();
                default_recomendation_algorithm(products,user,tiendaID);
            }        
        }


        public void Run(string tiendaID)
        {
            IDALUsuario udal = new DALUsuarioEF();
            IDALTienda tdal = new DALTiendaEF();
            IDALSubasta sdat = new DALSubastaEF();

            Tienda tienda = tdal.ObtenerTienda(tiendaID);
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
                if (defaultalgorithm)
                {
                    Algorithms def = new Algorithms();
                    def.default_recomendation_algorithm(productos, user, tienda.TiendaID);
                }
                else
                {
                    Algorithms a = new Algorithms();
                    a.custom_algorithm(pers, productos, user, tienda.TiendaID);
                }
            }//IF tienda es de instancia...
        }
    }
}
