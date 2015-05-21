using DataAccessLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chebay.BusinessLogicLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 5; //Cantidad de segundos a esperar.
            BLNotificaciones bl = new BLNotificaciones();
            IDALSubasta ip = new DALSubastaEF();
            IDALTienda it = new DALTiendaEF();
            IDictionary<string, DateTime> ultimosChequeos = new Dictionary<string, DateTime>();

            while (true)
            {
                //Obtengo todas las tiendas.
                List<Tienda> lt = it.ObtenerTodasTiendas();
                
                foreach (Tienda t in lt)
                try {
                    string idTienda = t.TiendaID;
                    Debug.WriteLine("idTienda: " + idTienda);

                    //Inicializo el ultimoChequeo
                    if (!ultimosChequeos.ContainsKey(idTienda))
                        ultimosChequeos.Add(idTienda, DateTime.Now);
                    
                    //Obtengo los productos que cerraron desde el último chequeo.
                    List<Producto> lp = ip.ObtenerProductosVencidos(ultimosChequeos[idTienda], DateTime.Now, idTienda);
                    ultimosChequeos[idTienda] = DateTime.Now;
                    Debug.WriteLine("ultimoChequeo: " + ultimosChequeos[idTienda].ToString());
                    
                    foreach (Producto p in lp)
                    {
                        Debug.WriteLine("p.productoID: " + p.ProductoID);
                        //Si fue comprado por CompraDirecta
                        if (p.compra != null)
                        {
                            Debug.WriteLine("p.compra != null");
                            //Notifico al ganador.
                            bl.sendEmailNotification(p.compra);
                        }
                        //Si fue comprado por Subasta.
                        else if (p.ofertas != null)
                        {
                            Debug.WriteLine("p.compra == null and p.ofertas != null");
                            //Obtengo la oferta ganadora.
                            Oferta o = ip.ObtenerOfertaGanadora(p.ProductoID, idTienda);
                            Debug.WriteLine("o.ofertaID: " + o.OfertaID);

                            //Creo una compra entre el Producto y el Usuario.
                            Compra c = new Compra
                            {
                                fecha_compra = p.fecha_cierre,
                                monto = o.monto,
                                ProductoID = o.ProductoID,
                                UsuarioID = o.UsuarioID
                            };
                            ip.AgregarCompra(c, idTienda);

                            //Notifico al ganador.
                            bl.sendEmailNotification(c);
                        }
                        //Si no fue comprado.
                        else
                        {

                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                
                //Espera n segundos.
                System.Threading.Thread.Sleep(n*1000);
            }
            
        }
    }
}
