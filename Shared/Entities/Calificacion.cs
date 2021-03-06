﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Calificaciones")]
    public class Calificacion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long ProductoID { get; set; }
        public string UsuarioEvalua { get; set; }
        public string UsuarioCalificado { get; set; }

        public int puntaje { get; set; }
        public string comentario { get; set; }
        [ForeignKey("ProductoID")]
        public virtual Producto producto { get; set; }
        [ForeignKey("UsuarioEvalua")]
        public virtual Usuario usuario_e { get; set; }
        [ForeignKey("UsuarioCalificado")]
        public virtual Usuario usuario_c { get; set; }
    }
}
