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
                //ChebayDBContext.ProvisionTenant("Tienda1");
                //ChebayDBContext.ProvisionTenant("Tienda2");
                //ChebayDBPublic.ProvidePublicSchema();

            /*
                using (var schema = ChebayDBPublic.CreatePublic())
                {
                    IDALTienda it = new DALTiendaEF();
                    it.AgregarAdministrador("idAdmin2", "pass123");
                    System.Console.WriteLine("ID      pass");
                    foreach (var a in schema.administradores.ToList())
                    {
                        System.Console.WriteLine(a.AdministradorID + "     " + a.password);
                    }
                    bool asdf = it.AutenticarAdministrador("idAdmin", "pass123");
                    System.Console.WriteLine(asdf);
                    it.AutenticarAdministrador("idAdmin", "pass1234");
                    System.Console.WriteLine(asdf);
                    it.AgregarTienda("Amazon", "desc1", "www.amazon.com", "idAdmin");


                    System.Console.WriteLine("");
                    System.Console.WriteLine("url    nombre      desc    admin");
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
                }

                //Ejemplo uso de un schema en particular
                using (var schema = ChebayDBContext.CreateTenant("Tienda1"))
                {


            
                }
            */
            
        }
    }
}