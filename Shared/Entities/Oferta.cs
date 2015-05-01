using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared.Entities
{
    [Table("Ofertas")]
    public class Oferta
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OfertaID { get; set; }
        [Required]
        public bool esFinal { get; set; }
        [Required]
        public int monto { get; set; }
        public long ProductoID { get; set; }
        public string UsuarioID { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto producto { get; set; }
        [ForeignKey("UsuarioID"), Required]
        public virtual Usuario usuario { get; set; }
    }
}
