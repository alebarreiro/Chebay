using System.ComponentModel.DataAnnotations.Schema;

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
