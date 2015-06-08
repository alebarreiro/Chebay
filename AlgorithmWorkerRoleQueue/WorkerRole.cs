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

namespace AlgorithmWorkerRoleQueue
{
    public class WorkerRole : RoleEntryPoint
    {
        // Nombre de la cola
        const string QueueName = "ProcessingQueue";
        const string TopicName = "algorithm";
        const string SubscriptionName = "AlgorithmSuscription";
        // QueueClient es seguro para subprocesos. Se recomienda almacenarlo en caché 
        // en lugar de crearlo de nuevo con cada solicitud
        //QueueClient Client;
        SubscriptionClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.WriteLine("Iniciando el procesamiento de mensajes");
            // Inicia el bombeo de mensajes y se invoca una devolución de llamada para cada mensaje que se recibe. Si se llama a close en el cliente, se detendrá el bombeo.
            //string roleName = "AlgorithmWorkerRoleQueue";
            //var endpoints = RoleEnvironment.CurrentRoleInstance.Id;
            //Debug.WriteLine("");
            int instances = int.Parse(CloudConfigurationManager.GetSetting("Instances"));
            //Debug.WriteLine("NUMERO DE INSTANCIAS::" + i);
            //Debug.WriteLine("");

            int instance = 0;
            string instanceId = RoleEnvironment.CurrentRoleInstance.Id;

            bool ok = int.TryParse(instanceId.Substring(instanceId.LastIndexOf("_")+1), out instance);
            if (!ok)
            {
                Debug.WriteLine("ERROR! INSTANCIA NO VALIDA");
                instance = 0;
            }

            Client.OnMessage((receivedMessage) =>
                {
                    try
                    {

                        if (receivedMessage != null)
                        {
                            //procesar
                            //Debug.WriteLine("COMENZO ALGORITMO!!!");
                            Algorithms alg = new Algorithms();
                            alg.Run(instance, instances);
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
            // Establecer el número máximo de conexiones concurrentes. 
            ServicePointManager.DefaultConnectionLimit = 12;
            string instanceId = RoleEnvironment.CurrentRoleInstance.Id;

            instanceId = instanceId.Substring(instanceId.LastIndexOf("_") + 1);

            // Crear la cola si no existe aún
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.TopicExists(TopicName))
            {
                namespaceManager.CreateTopic(TopicName);
            }

            if (!namespaceManager.SubscriptionExists(TopicName, SubscriptionName+instanceId))
            {
               namespaceManager.CreateSubscription(TopicName, SubscriptionName+instanceId);
            }
            // Creates the subscription client
            Client = SubscriptionClient.CreateFromConnectionString(connectionString, TopicName, SubscriptionName+instanceId);
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