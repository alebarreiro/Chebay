using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    public enum MsgType
    {
        BROADCAST,
        PRIVATE
    }

    [Table("Mensajes")]
    public class Mensaje
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string contenido { get; set; }
        public DateTime fecha { get; set; }
        public MsgType tipo { get; set; }

        [ForeignKey("emisor")]
        public string usuarioEmisor { get; set; }
        [ForeignKey("receptor")]
        public string usuarioReceptor { get; set; }
        //[ForeignKey("conversacion")]
        public long ConversacionID { get; set; }


        public virtual Usuario emisor { get; set; }
        //null if broadcast
        public virtual Usuario receptor { get; set; }
        public virtual Conversacion conversacion { get; set; }
    }
}
