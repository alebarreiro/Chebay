using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Conversaciones")]
    public class Conversacion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ConversacionID { get; set; }
        public virtual ICollection<Mensaje> mensajes { get; set; }
    }
}
