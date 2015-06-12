using Shared.DataTypes;
using Shared.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Chebay.AlgorithmDLL
{
    public class ChebayAlgorithm : IChebayAlgorithm
    {
        //only read!
        //determinate the best products por user
        public List<DataProducto> getProducts(List<Producto> products, Usuario user)
        {
            List<DataProducto> ret = new List<DataProducto>();
            var query = from p in products
                        orderby p.nombre
                        select p;
            
            foreach (var p in query.ToList())
            {               
                ret.Add(new DataProducto(p));
            }
            return ret;
        }
    }
}
