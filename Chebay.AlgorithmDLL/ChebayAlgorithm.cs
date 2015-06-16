using Shared.DataTypes;
using Shared.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Chebay.AlgorithmDLL
{
    public class ChebayAlgorithm : IChebayAlgorithm
    {
        //only read!
        //toma los 5 productos mas costosos de la tienda
        private List<DataProducto> getByCost(List<Producto> products, Usuario user)
        {
            List<DataProducto> ret = new List<DataProducto>();
            var query = (from p in products
                         orderby p.precio_compra descending
                         select p).Take(5);
            foreach (var i in query)
            {
                ret.Add(new DataProducto(i));
            }
            return ret;
        }

        private List<DataProducto> getByFavoriteCategory(List<Producto> products, Usuario user)
        {
            
            List<DataProducto> ret = new List<DataProducto>();
            var query = (from p in user.favoritos
                         group p by p.CategoriaID into grupo
                         select new { catid = grupo.FirstOrDefault().CategoriaID, count = grupo.Count() })
                       .OrderByDescending(x => x.count);
            foreach (var i in query)
            {
                System.Console.WriteLine(i.catid+" "+ i.count);
            }
            if (query.Count() == 0)
            {
                //si no tiene favoritos retorna primeros 3 productos categoria samsung
                var q = (from p in products
                        where p.CategoriaID==2
                        select p).Take(3);
                foreach (var p in q)
                {
                    ret.Add(new DataProducto(p));
                }
                return ret;
            }
            var mostfav = (long)query.FirstOrDefault().catid;
            foreach (var p in products)
            {
                if (p.CategoriaID == mostfav)
                    ret.Add(new DataProducto(p));
            }
            return ret;
        }

        public List<DataProducto> getProducts(List<Producto> products, Usuario user)
        {
            return getByCost(products, user);
                //getByFavoriteCategory(products, user);
        }
    }
}



/* por favoritos categoria
 
            List<DataProducto> ret = new List<DataProducto>();
            var query = (from p in user.favoritos
                         group p by p.CategoriaID into grupo
                         select new { catid = grupo.FirstOrDefault().CategoriaID, count = grupo.Count() })
                       .OrderByDescending(x => x.count);
            foreach (var i in query)
            {
                System.Console.WriteLine(i.catid+" "+ i.count);
            }
            var mostfav = (long)query.FirstOrDefault().catid;
            foreach (var p in products)
            {
                if (p.CategoriaID == mostfav)
                    ret.Add(new DataProducto(p));
            }
            return ret;
        }
 
 */
