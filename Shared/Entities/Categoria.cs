using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Categorias")]
    public abstract class Categoria
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CategoriaID { get; set; }
        [Required]
        public string Nombre { get; set; }

        public virtual ICollection<TipoAtributo> tipoatributos { get; set; }
        public virtual CategoriaCompuesta padre { get; set; }
    }
}
