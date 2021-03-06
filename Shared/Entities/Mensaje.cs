﻿using System;
using System.ComponentModel.DataAnnotations;
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
        public long MensajeID { get; set; }
        [Required]
        public string contenido { get; set; }
        [Required]
        public DateTime fecha { get; set; }
        [Required]
        public MsgType tipo { get; set; }

        public string usuarioEmisor { get; set; }
        public string usuarioReceptor { get; set; }
        public long ConversacionID { get; set; }

        [ForeignKey("usuarioEmisor"), Required]
        public virtual Usuario emisor { get; set; }
        //null if broadcast
        [ForeignKey("usuarioReceptor")]
        public virtual Usuario receptor { get; set; }
        [ForeignKey("ConversacionID")]
        public virtual Conversacion conversacion { get; set; }
    }
}
