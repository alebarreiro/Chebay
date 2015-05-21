using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace Chebay.AlgorithmDLL
{
    interface IChebayAlgorithm
    {
        List<Producto> getProducts(List<Producto> productos, Usuario user);
    }
}
