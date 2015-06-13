using DataAccessLayer;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using System;

namespace WebAlgorithm
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        
        //Hace broadcast a topic, para todas las instancias del worker role.
        static void Main()
        {
            string QueueName = "recomendacion";       
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            QueueClient Client;
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);

            IDALTienda tdal = new DALTiendaEF();
            var tiendas = tdal.ObtenerTodasTiendas();
            foreach (var t in tiendas)
            {
                Console.WriteLine(t.TiendaID);
                var message = new BrokeredMessage(t.TiendaID);
                Client.Send(message);
            }
        }
    }
}
