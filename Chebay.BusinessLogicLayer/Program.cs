using DataAccessLayer;
using Shared.Entities;
using System;
using Chebay.DataAccessLayerTests;

namespace Chebay.BusinessLogicLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            IDALMercadoLibreREST ml = new DALMercadoLibreREST();
            ChebayDBPublic.ProvidePublicSchema();
            using (var db = ChebayDBPublic.CreatePublic())
            {
                db.Seed();
            }

            string tenant = "MobileCenter";
            ChebayDBContext.ProvisionTenant(tenant);
            using (var db = ChebayDBContext.CreateTenant(tenant))
            {
                db.SeedMobileCenter();
            }
            Console.Read();
        }
    }
}
