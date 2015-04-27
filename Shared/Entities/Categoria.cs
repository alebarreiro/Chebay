using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Categorias")]
    public abstract class Categoria
    {
        //id
        public string CategoriaID { get; set; }
   
        public ICollection<Atributo> atributos { get; set; }

        //puede ser null
        public virtual CategoriaCompuesta padre { get; set; }
    }
}
