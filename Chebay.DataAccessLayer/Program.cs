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
            Console.WriteLine("Utilizar en caso de pruebas minimas...");
            //ChebayDBPublic.ProvidePublicSchema();

            //using (var context = ChebayDBPublic.CreatePublic())
            //    {
            //       context.Seed();
            //    }
      /*      AtributoSesion a = new AtributoSesion { AdministradorID="test@chebay.com", AtributoSesionID="sesion", Datos="esta nueva"};
            IDALTienda dal = new DALTiendaEF();
            dal.AgregarAtributoSesion(a);
            List<AtributoSesion> list = dal.ObtenerAtributosSesion("test@chebay.com");
            foreach (var l in list)
            {
                Console.WriteLine(l.AtributoSesionID+" "+l.AdministradorID+ " "+ l.Datos);
            }*/
            Console.Read();
            //Ejemplo para crear schema
            //ChebayDBContext.ProvisionTenant("Tienda1");
            //ChebayDBPublic.ProvidePublicSchema();
        
            //Ejemplo utilizar schema
            //using (var context = ChebayDBContext.CreateTenant("Tienda1"))
            //{
            //}
        }
           
    }
}
