using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using Shared.DataTypes;

namespace Chebay.AlgorithmDLL
{
    public class ChebayAlgorithm : IChebayAlgorithm
    {
        //only read!
        //determinate the best products por user
        public List<DataProducto> getProducts(List<Producto> products, Usuario user)
        {
            System.Console.WriteLine("Algoritmo ha comenzado!");
            List<DataProducto> ret = new List<DataProducto>();
            var query = from p in products
                        orderby p.nombre
                        select p;
            
            foreach (var p in query.ToList())
            {               
                ret.Add(new DataProducto(p));
            }
            //joke server :)
            //while (true) ;
            //return null;
            return ret;
        }
    }
}
