using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    //perform shearch
    [Table("Conversaciones")]
    public class Conversacion
    {
        public long ConversacionID { get; set; }
        public virtual ICollection<Mensaje> mensajes { get; set; }
    }
}
