using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared.Entities
{
    [Table("Compras")]
    public class Compra
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long CompraID { get; set; }
        [Required]
        public int monto { get; set; }
        public DateTime fecha_compra { get; set; }

        [Key, ForeignKey("producto")]
        public long ProductoID { get; set; }
        public string UsuarioID { get; set; }

        public virtual Producto producto { get; set; }
        [ForeignKey("UsuarioID"), Required]
        public virtual Usuario usuario { get; set; }
    }
}
