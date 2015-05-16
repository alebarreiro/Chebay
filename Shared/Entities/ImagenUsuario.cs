using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class ImagenUsuario
    {
        [Key, ForeignKey("usuario")]
        public string UsuarioID { get; set; }
        public byte[] Imagen { get; set; }

        public virtual Usuario usuario { get; set; }
    }
}
