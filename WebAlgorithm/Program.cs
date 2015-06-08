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


        //Hace broadcast a topic, para todas las instancias del worker role.
        static void Main()
        {
            string TopicName = CloudConfigurationManager.GetSetting("TopicString"); // "algorithm";

            // get the connection string from config (app.config in this sample)
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            TopicClient Client = TopicClient.CreateFromConnectionString(connectionString, TopicName);
            BrokeredMessage message = new BrokeredMessage(DateTime.Now.ToString());
            // Send message to the topic
            Client.Send(message);
            System.Console.WriteLine("Message Send...");
            System.Console.Read();

        }
    }
}
