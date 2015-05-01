using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Entities
{
    [Table("Visitas")]
    public class Visita
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VisitaID { get; set; }
        public string UsuarioID { get; set; }
        public long ProductoID { get; set; }
        public virtual Producto producto { get; set; }
        public virtual Usuario usuario { get; set; }
    }
}
