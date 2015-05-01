using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Atributos")]
    public class Atributo
    {
        //etiqueta
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AtributoID { get; set; }
        public string CategoriaID { get; set; }
        public string valor { get; set; }

        public virtual Categoria categorias { get; set; }
    }
}
