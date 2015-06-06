using DataAccessLayer;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;

namespace Chebay.BusinessLogicLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            BLNotificaciones not = new BLNotificaciones();
            Compra c = new Compra { ProductoID=1, UsuarioID="joleocl@gmail.com", monto=2, fecha_compra= DateTime.UtcNow};
            DataProductoQueue dp = new DataProductoQueue { nombre = "prueba", OwnerProducto = "joleocl@gmail.com" , fecha_cierre=DateTime.UtcNow};
            not.sendEmailNotification(c, dp);

            /*
            QueueClient Client;
            // Establecer el número máximo de conexiones concurrentes. 
            ServicePointManager.DefaultConnectionLimit = 12;
            string QueueName = "notificacionescompra";
            // Crear la cola si no existe aún
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Inicializar la conexión con la cola de Service Bus
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);

            var message = new BrokeredMessage("algo") { ScheduledEnqueueTimeUtc= DateTime.UtcNow.AddMinutes(1)};
            Client.Send(message);
            System.Console.WriteLine(DateTime.UtcNow.AddMinutes(1).ToString());
            System.Console.Read();
            */
            
            
            
            //BLPersonalizacion blp = new BLPersonalizacion();
            //blp.PersonalizarTienda("e44d26", "f16529", 2, "HardPC");
            /*
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
                        if (ip.ExisteCompra(p.ProductoID,idTienda))
                        {
                            //Notifico al ganador.
                            bl.sendEmailNotification(ip.ObtenerCompra(p.ProductoID, idTienda), p);
                        }
                        //Si fue comprado por Subasta.
                        else if (p.ofertas != null)
                        {
                            //Obtengo la oferta ganadora.
                            Oferta o = ip.ObtenerOfertaGanadora(p.ProductoID, idTienda);

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
                            //bl.sendEmailNotification(c);
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
            */
        }
    }
}
