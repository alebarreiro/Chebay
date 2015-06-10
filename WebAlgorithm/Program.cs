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
        
        //Hace broadcast a topic, para todas las instancias del worker role.
        static void Main()
        {
            string QueueName = "recomendation";       
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
