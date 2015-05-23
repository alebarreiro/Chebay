using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using Shared.DataTypes;

namespace Chebay.AlgorithmDLL
{
    interface IChebayAlgorithm
    {
        List<DataProducto> getProducts(List<Producto> productos, Usuario user);
    }
}
