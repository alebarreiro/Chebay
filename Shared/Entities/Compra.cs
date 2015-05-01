using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Compras")]
    public class Compra
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public int monto { get; set; }

        //[ForeignKey("producto")]
        public long ProductoID { get; set; }
        //[ForeignKey("usuario")]
        public string UsuarioID { get; set; }

        public virtual Producto producto { get; set; }
        public virtual Usuario usuario { get; set; }
    }
}
