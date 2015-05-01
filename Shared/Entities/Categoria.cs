using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared.Entities
{
    [Table("Categorias")]
    public abstract class Categoria
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CategoriaID { get; set; }
        [Required]
        public string Nombre { get; set; }
        //public long PadreID { get; set; }
   
        public ICollection<Atributo> atributos { get; set; }

        //puede ser null
        public virtual CategoriaCompuesta padre { get; set; }
    }
}
