using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("UsuarioID")]
        public virtual Usuario usuario { get; set; }
    }
}
