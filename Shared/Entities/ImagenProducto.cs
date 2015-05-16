using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class ImagenProducto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long ProductoID { get; set; }
        public byte[] Imagen { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto producto { get; set; }
    }
}
