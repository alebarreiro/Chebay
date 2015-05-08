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
                ChebayDBContext.ProvisionTenant("PhoneBayMOD3");

                //using (var schema = ChebayDBPublic.CreatePublic())
                //{
                //    schema.Seed();
                //}

                using (var schema = ChebayDBContext.CreateTenant("PhoneBayMOD3"))
                {
                    schema.seed();
                }

         /*       using (var schema = ChebayDBContext.CreateTenant("PhoneBay"))
                {
                    //var query = from cat in schema.categorias
                    //            where cat.CategoriaID ==1
                    //            select cat;
                    //CategoriaCompuesta father =  (CategoriaCompuesta)query.FirstOrDefault();
                    //System.Console.WriteLine(father.CategoriaID+father.Nombre);
                    //Categoria c = new CategoriaCompuesta { Nombre="testing", padre=father};
                    //schema.categorias.Add(c);
                    //schema.SaveChanges();
                    var query = from cat in schema.categorias
                                where cat.CategoriaID == 1
                                select cat;
                    CategoriaCompuesta c = (CategoriaCompuesta)query.FirstOrDefault();
                    foreach (var hijos in c.hijas)
                    {
                        System.Console.WriteLine(hijos.CategoriaID+hijos.Nombre);
                    }
                    Console.Read();
            */
            /*
                using (var schema = ChebayDBContext.CreateTenant("TestURL"))
                {
                    var query = from cat in schema.categorias
                                where cat.CategoriaID ==1
                                select cat;
                    CategoriaCompuesta father =  (CategoriaCompuesta)query.FirstOrDefault();
                    System.Console.WriteLine(father.CategoriaID+father.Nombre);
                    Categoria c = new CategoriaCompuesta { Nombre="testing3", padre=father};
                    System.Console.WriteLine(c.CategoriaID + c.Nombre);
                    schema.categorias.Add(c);
                    schema.SaveChanges();
                }
             */
                    Console.Read();

                    //schema.seed();
                }
        //
            
                //using (var schema = ChebayDBPublic.CreatePublic())
                //{
                //    IDALTienda it = new DALTiendaEF();
                //   // it.AgregarAdministrador("idAdmin2", "pass123");
                //    System.Console.WriteLine("ID      pass");
                //    foreach (var a in schema.administradores.ToList())
                //    {
                //        System.Console.WriteLine(a.AdministradorID + "     " + a.password);
                //    }
                    
                //    foreach (var a in schema.tiendas.ToList())
                //    {
                //        System.Console.Write(a.TiendaID +
                //            "     " + a.nombre +
                //            "   " + a.descripcion);
                //        foreach (var adm in a.administradores.ToList())
                //        {
                //            System.Console.WriteLine("    " + adm.AdministradorID);
                //        }
                //    }
                //    System.Console.ReadLine();
                //}
            

            
            
        }
    }
