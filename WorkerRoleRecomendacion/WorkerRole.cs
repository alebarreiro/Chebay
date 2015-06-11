using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace WorkerRoleRecomendacion
{
    public class WorkerRole : RoleEntryPoint
    {
        // Nombre de la cola
        const string QueueName = "recomendacion";

        // QueueClient es seguro para subprocesos. Se recomienda almacenarlo en caché 
        // en lugar de crearlo de nuevo con cada solicitud
        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.WriteLine("Iniciando el procesamiento de mensajes");

            // Inicia el bombeo de mensajes y se invoca una devolución de llamada para cada mensaje que se recibe. Si se llama a close en el cliente, se detendrá el bombeo.
            Client.OnMessage((receivedMessage) =>
                {
                    try
                    {
                        string tienda = receivedMessage.GetBody<string>();
                        // Procesar el mensaje
                        Trace.WriteLine("Procesando tienda: " + tienda);
                        Algorithms a = new Algorithms();
                        a.Run(tienda);
                    }
                    catch
                    {
                        Trace.WriteLine("ERROR PARSING String...");
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