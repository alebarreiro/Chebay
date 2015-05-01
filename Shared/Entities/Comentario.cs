using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Comentarios")]
    public class Comentario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ComentarioID { get; set; }
        public string texto { get; set; }
        public DateTime fecha { get; set; }

        //[ForeignKey("producto")]
        public long ProductoID { get; set; }
        //[ForeignKey("usuario")]
        public string UsuarioID { get; set; }

        public virtual Producto producto { get; set; }
        public virtual Usuario usuario { get; set; }
    }
}
