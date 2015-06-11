using System.Collections.Generic;

namespace Shared.Entities
{
    public class CategoriaSimple : Categoria
    {
        public virtual ICollection<Producto> productos { get; set; }
    }
}
