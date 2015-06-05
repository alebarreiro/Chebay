using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using DataAccessLayer;
using Shared.Entities;
using System.Threading;
using Shared.DataTypes;
using Microsoft.WindowsAzure;
using Microsoft.ServiceBus.Messaging;

namespace WebAlgorithm
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static int _threads = 2;
        static void Run(object data)
        {
            Algorithms a = new Algorithms();
            a.Run((int)data, _threads);
        }

        static void Main()
        {
            //var host = new JobHost();
            // The following code ensures that the WebJob will be running continuously
            
            


            //Thread[] pool = new Thread[_threads];
            //for (int i = 0; i < _threads; i++)
            //{
            //    pool[i] = new Thread(Run);
            //    pool[i].Start(i);
            //}

            //host.RunAndBlock();


            string TopicName = "algorithm";

             DataCalificacion model = new DataCalificacion();
 
             // prepare the message
             model.cantCalificaciones=10;
             model.promedio=2;
             // get the connection string from config (app.config in this sample)
             string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

             TopicClient Client = TopicClient.CreateFromConnectionString(connectionString, TopicName);
 
             // Create message, passing our model
             BrokeredMessage message = new BrokeredMessage(model);

             // Send message to the topic
             Client.Send(message);


             const string SubscriptionName = "AlgorithmSuscription";
             // QueueClient es seguro para subprocesos. Se recomienda almacenarlo en caché 
             // en lugar de crearlo de nuevo con cada solicitud
             //QueueClient Client;
             SubscriptionClient ClientR;
             ClientR = SubscriptionClient.CreateFromConnectionString(connectionString, TopicName, SubscriptionName);

             BrokeredMessage bm = ClientR.Receive();
             System.Console.WriteLine(bm.GetBody<DataCalificacion>());
             System.Console.Read();


        }
    }
}
