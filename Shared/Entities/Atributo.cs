using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Atributos")]
    public class Atributo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AtributoID { get; set; }
        public string TipoAtributoID { get; set; }
        [Required]
        public string etiqueta { get; set; }
        [Required]
        public string valor { get; set; }

        [ForeignKey("TipoAtributoID")]
        public virtual TipoAtributo tipoatributo { get; set; }
        public virtual ICollection<Producto> productos { get; set; }
    }
}
