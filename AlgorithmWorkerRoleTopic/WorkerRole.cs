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
using System.Configuration;

namespace AlgorithmWorkerRoleTopic
{
    public class WorkerRole : RoleEntryPoint
    {
        // Nombre de la cola
        const string QueueName = "Recomendation";

        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.WriteLine("Iniciando el procesamiento de mensajes");

            Client.OnMessage((receivedMessage) =>
            {
                try
                {
                    if (receivedMessage != null)
                    {
                        //procesar
                        Algorithms alg = new Algorithms();
                        //alg.Run(instance, instances);
                        receivedMessage.Complete();
                    }
                    // Procesar el mensaje
                    Trace.WriteLine("Procesando el mensaje de Service Bus: " + receivedMessage.SequenceNumber.ToString());
                }
                catch
                {
                    // Controlar cualquier excepción específica del procesamiento de mensajes aquí
                }
            });

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            Debug.WriteLine("ALgorithm SLEEPING...");
            Thread.Sleep(500000);
            // Establecer el número máximo de conexiones concurrentes. 
            ServicePointManager.DefaultConnectionLimit = 12;
            string instanceId = RoleEnvironment.CurrentRoleInstance.Id;

            instanceId = instanceId.Substring(instanceId.LastIndexOf("_") + 1);

            // Crear la cola si no existe aún
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }
            // Creates the subscription client
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