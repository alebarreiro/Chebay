using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using DataAccessLayer;
using Shared.Entities;
using Shared.DataTypes;

namespace AlgorithmWorkRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        static private int minutes =60;
        static private int time = 60000 * minutes;
        static int workerRoleInstance = 2;

        public override void Run()
        {
            Trace.TraceInformation("AlgorithmWorkRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
                Debug.WriteLine("Sleep " + minutes + " minuto");
                Thread.Sleep(time);
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Establecer el número máximo de conexiones simultáneas
            ServicePointManager.DefaultConnectionLimit = 12;

            // Para obtener información sobre cómo administrar los cambios de configuración
            // consulte el tema de MSDN en http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("AlgorithmWorkRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("AlgorithmWorkRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("AlgorithmWorkRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            string instanceId = RoleEnvironment.CurrentRoleInstance.Id;
            int instance = 0;

            bool ok = int.TryParse(instanceId.Substring(instanceId.LastIndexOf("_")+1), out instance);
            if (!ok)
            {
                Debug.WriteLine("ERROR! INSTANCIA NO VALIDA");
                instance = 0;
            }


            IDALUsuario udal = new DALUsuarioEF();
            IDALTienda tdal = new DALTiendaEF();
            IDALSubasta sdat = new DALSubastaEF();

            List<Tienda> tiendas = tdal.ObtenerTodasTiendas();

            foreach (var tienda in tiendas)
            {
                //hardocoded numero instancias.

                //Debug.WriteLine("NUMERO DE INSTANCIAS.." + workerRoleInstance);
                //tiene asignado algunas instancias de la tienda.
                
                //Debug.WriteLine(tienda.TiendaID+"MOD::"+tienda.TiendaID.GetHashCode() % workerRoleInstance);
                if (Math.Abs(tienda.TiendaID.GetHashCode() % workerRoleInstance) == instance)
                {
                    Debug.WriteLine("Instance::"+instance+"::"+tienda.TiendaID);
                    Debug.WriteLine("TiendaHash: " + tienda.TiendaID.GetHashCode());


                    List<Producto> productos = sdat.ObtenerTodosProductos(tienda.TiendaID);
                    //obtengo algoritmo
                    Personalizacion pers = tdal.ObtenerPersonalizacionTienda(tienda.TiendaID);
                    List<Usuario> usuarios = udal.ObtenerTodosUsuariosFull(tienda.TiendaID);
                    bool defaultalgorithm = false;
                    
                    if (pers.algoritmo == null || pers.algoritmo.Length == 0)
                    {
                        defaultalgorithm = true;
                    }


                    //creo indice
                    Task index = udal.InicializarColeccionRecomendaciones(tienda.TiendaID);
                    index.Wait();

                    foreach (var user in usuarios)
                    {
                        //Debug.WriteLine("USUARIO::" + user.UsuarioID);
                        if (defaultalgorithm)
                        {
                            Algorithms def = new Algorithms();
                            Thread defThread = new Thread(() =>
                            {
                                def.default_recomendation_algorithm(productos, user, tienda.TiendaID);
                            });
                            defThread.Start();
                        }
                        else
                        {
                            Thread t = new Thread(
                                delegate()
                                {
                                    //Debug.WriteLine("CustomAlgorithm");
                                    Algorithms a = new Algorithms();
                                    a.custom_algorithm(pers, productos, user, tienda.TiendaID);
                                });
                            t.Start();

                        }
                }//IF tienda es de instancia...
                }
            }

        }
    }
}
