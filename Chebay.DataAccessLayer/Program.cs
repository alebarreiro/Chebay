using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Shared.Entities;
using System.Data.Entity.Core.EntityClient;
using System.Data.Common;
using System.Diagnostics;

namespace DataAccessLayer
{
    class Program
    {
        static void Main(string[] args)
        {
                //Creo schemas

                //Ejemplo para crear schema
          //      ChebayDBContext.ProvisionTenant("Tienda1");
          //      ChebayDBContext.ProvisionTenant("Tienda2");
                //ChebayDBPublic.ProvidePublicSchema();

            
                using (var schema = ChebayDBPublic.CreatePublic())
                {
                    IDALTienda it = new DALTiendaEF();
                   // it.AgregarAdministrador("idAdmin2", "pass123");
                    System.Console.WriteLine("ID      pass");
                    foreach (var a in schema.administradores.ToList())
                    {
                        System.Console.WriteLine(a.AdministradorID + "     " + a.password);
                    }
                    
                    foreach (var a in schema.tiendas.ToList())
                    {
                        System.Console.Write(a.TiendaID +
                            "     " + a.nombre +
                            "   " + a.descripcion);
                        foreach (var adm in a.administradores.ToList())
                        {
                            System.Console.WriteLine("    " + adm.AdministradorID);
                        }
                    }
                    System.Console.ReadLine();
                }
            
                //Ejemplo uso de un schema en particular
             //   using (var schema = ChebayDBContext.CreateTenant("Tienda1"))
              //  {
              //      schema.seed();
              //  }
            
            
        }
    }
}