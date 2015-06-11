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

            CustomTests test = new CustomTests();
            test.CierreSubastaCompraDirecta();
            test.CierreSubastaConOferta();
            test.CierreSubastaNoCompras();

            Console.Read();
        }
    }
}
