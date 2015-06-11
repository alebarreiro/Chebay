using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    public enum TipoDato {
        INTEGER,
        DATE,
        BOOL,
        STRING,
        FLOAT,
        BINARY
    };

    [Table("TipoAtributos")]
    public class TipoAtributo
    {
        [Key]
        public string TipoAtributoID { get; set; }
        [Required]
        public TipoDato tipodato { get; set; }

        public virtual ICollection<Atributo> atributos { get; set; }
        public virtual ICollection<Categoria> categorias { get; set; }

    }
}
