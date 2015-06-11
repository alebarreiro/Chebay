using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Compras")]
    public class Compra
    {
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
