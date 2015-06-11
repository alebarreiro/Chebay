using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Comentarios")]
    public class Comentario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ComentarioID { get; set; }
        [Required]
        public string texto { get; set; }
        public DateTime fecha { get; set; }

        public long ProductoID { get; set; }
        public string UsuarioID { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto producto { get; set; }
        [ForeignKey("UsuarioID")]
        public virtual Usuario usuario { get; set; }

    }
}
