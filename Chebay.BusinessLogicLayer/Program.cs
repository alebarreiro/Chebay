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
            CustomTests ct = new CustomTests();
            ct.CierreSubastaCompraDirecta();
            Console.Read();
        }
    }
}
