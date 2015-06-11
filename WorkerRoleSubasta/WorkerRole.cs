using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Shared.DataTypes;
using Shared.Entities;
using DataAccessLayer;
using Microsoft.Azure;

namespace WorkerRoleSubasta
{
    public class WorkerRole : RoleEntryPoint
    {
        // Nombre de la cola
        const string QueueName = "subasta";

        // QueueClient es seguro para subprocesos. Se recomienda almacenarlo en caché 
        // en lugar de crearlo de nuevo con cada solicitud
        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);


        private void procesarSubasta(DataProductoQueue p)
        {
            IDALSubasta ip = new DALSubastaEF();
            BLNotificaciones bl = new BLNotificaciones();
            IDALUsuario ubl = new DALUsuarioEF();

            //Si no fue comprado por CompraDirecta
            if (!ip.ExisteCompra(p.ProductoID, p.TiendaID))
            {
                //vendedor
                Usuario vendedor = ubl.ObtenerUsuario(p.OwnerProducto, p.TiendaID);

                if (ip.TieneOfertas(p.ProductoID, p.TiendaID))
                {
                    //Obtengo la oferta ganadora.
                    Oferta o = ip.ObtenerOfertaGanadora(p.ProductoID, p.TiendaID);

                    //Creo una compra entre el Producto y el Usuario.
                    Compra c = new Compra
                    {
                        fecha_compra = p.fecha_cierre,
                        monto = o.monto,
                        ProductoID = o.ProductoID,
                        UsuarioID = o.UsuarioID
                    };
                    //se envia mensaje al vendedor en AgregarCompra
                    ip.AgregarCompra(c, p.TiendaID);

                    //obtengo mail ganador
                    
                    Usuario u = ubl.ObtenerUsuario(c.UsuarioID, p.TiendaID);
                    //Notifico al ganador.
                    string mensaje = "<p>Has ganado una subasta sobre el producto "
                                     + p.ProductoID + " " + p.nombre + "!</p>" +
                                     "<p> Te invitamos a calificar al vendedor accediendo al siguiente enlace: "
                                     + " http://chebuynow.azurewebsites.net/" + p.TiendaID + "/Usuario/CalificarUsuario?prodId=" + p.ProductoID + " </p>";
                    string asunto = "¡Has ganado una subasta en Chebay!";
                    bl.sendEmailNotification(u.Email, asunto, mensaje);
                }
                else
                {
                    //no fue comprado
                    Debug.WriteLine("NO TIENE COMPRAS MAIL AL VENDEDOR");
                    string asunto = "Producto " + p.nombre;
                    string mensaje = "<p>Lo sentimos... El producto " + p.ProductoID + " " + p.nombre + " ha alcanzado su fecha de finalizacion y no se ha vendido.</p>";
                    bl.sendEmailNotification(vendedor.Email, asunto, mensaje);
                }
                
            }//si fue comprado se descarta el mensaje.
         
        }


        public override void Run()
        {
            Trace.WriteLine("Iniciando el procesamiento de mensajes");

            // Inicia el bombeo de mensajes y se invoca una devolución de llamada para cada mensaje que se recibe. Si se llama a close en el cliente, se detendrá el bombeo.
            Client.OnMessage((receivedMessage) =>
                {
                    try
                    {
                        // Procesar el mensaje
                        DataProductoQueue message = receivedMessage.GetBody<DataProductoQueue>();
                        Trace.WriteLine("Procesando mensaje del producto " + message.nombre);
                        procesarSubasta(message);
                    }
                    catch
                    {
                        Trace.WriteLine("ERROR AL PROCESAR MENSAJE QUEUE SUBASTA...");
                        // Controlar cualquier excepción específica del procesamiento de mensajes aquí
                    }
                });

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            // Establecer el número máximo de conexiones concurrentes. 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Crear la cola si no existe aún
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Inicializar la conexión con la cola de Service Bus
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Cerrar la conexión con la cola de Service Bus
            Client.Close();
            CompletedEvent.Set();
            base.OnStop();
        }
    }
}