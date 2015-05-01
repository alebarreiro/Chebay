using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared.Entities
{
    [Table("Atributos")]
    public class Atributo
    {
        //etiqueta
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AtributoID { get; set; }
        public long CategoriaID { get; set; }
        [Required]
        public string etiqueta { get; set; }
        [Required]
        public string valor { get; set; }

        [ForeignKey("CategoriaID")]
        public virtual Categoria categorias { get; set; }
    }
}
