using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class CategoriaCompuesta : Categoria
    {
        public virtual ICollection<Categoria> hijas { get; set; }
        //public virtual Tienda tienda { get; set; }
    }
}
