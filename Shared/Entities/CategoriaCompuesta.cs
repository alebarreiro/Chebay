using System.Collections.Generic;

namespace Shared.Entities
{
    public class CategoriaCompuesta : Categoria
    {
        public virtual ICollection<Categoria> hijas { get; set; }
    }
}
