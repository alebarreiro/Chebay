using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using DataAccessLayer;
using Shared.Entities;
using System.Threading;

namespace WebAlgorithm
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            //var host = new JobHost();
            // The following code ensures that the WebJob will be running continuously
            int threads = 2;
            for (int i = 0; i < threads; i++)
            {
                Thread t = new Thread(delegate(){
                    Algorithms al = new Algorithms();
                    al.Run(i, threads);            
                });
                t.Start();
                System.Console.WriteLine("New thread "+ i);
            }



            //host.RunAndBlock();
        }
    }
}
