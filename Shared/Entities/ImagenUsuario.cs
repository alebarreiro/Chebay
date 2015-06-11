using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
