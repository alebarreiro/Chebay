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
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

            using (var db = new SqlConnection(connection))
            {
                //Creo schemas

                //Ejemplo para crear schema
                //ChebayDBContext.ProvisionTenant("Tienda1", db);
                //ChebayDBContext.ProvisionTenant("Tienda2", db);
                //ChebayDBPublic.ProvidePublicSchema(db);


                using (var schema = ChebayDBPublic.CreatePublic(db))
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
                }

                //Ejemplo uso de un schema en particular
                using (var schema = ChebayDBContext.CreateTenant("Tienda1", db))
                {
                    //ejemplo listar productos
                    var query = from p in schema.productos
                                select p;
                    foreach (var p in query.ToList())
                    {
                        System.Console.WriteLine(p.nombre);
                    }
                    
                    System.Console.Read();

                    //Ejemplos agregar atributo y administrador
                    /*
                    Atributo a = new Atributo();
                    a.AtributoID = "1";
                    a.categoria = null;
                    a.valor = "valor";
                    schema.atributos.Add(a);
                    schema.SaveChanges();
                     */
                    //Administrador a = new Administrador();
                    //a.AdministradorID = "Admin4";
                    //a.password = "secret";
                    //schema.administradores.Add(a);
                    //schema.SaveChanges();
                }

            }
        }
    }
}