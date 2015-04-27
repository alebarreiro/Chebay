using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Calificaciones")]
    public class Calificacion
    {
        public long ID { get; set; }
        public int puntaje { get; set; }

        [ForeignKey("usuario_e")]
        public string UsuarioEvalua { get; set; }
        [ForeignKey("usuario_c")]
        public string UsuarioCalificado { get; set; }

        public virtual Usuario usuario_e { get; set; }
        public virtual Usuario usuario_c { get; set; }
    }
}
