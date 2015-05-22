using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace Chebay.AlgorithmDLL
{
    public class ChebayAlgorithm : IChebayAlgorithm
    {
        //only read!
        //determinate the best products por user
        public List<Producto> getProducts(List<Producto> products, Usuario user)
        {
            /*
            var query = from p in products
                        orderby p.nombre
                        select p;
             */
            //joke server :)
            while (true) ;
            return null;
            //return query.ToList();
        }
    }
}
