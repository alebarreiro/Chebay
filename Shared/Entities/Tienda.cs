using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Tiendas")]
    public class Tienda
    {
        public string TiendaID { get; set; }
        [Required]
        public string nombre { get; set; }
        public string descripcion { get; set; }

        public virtual ICollection<Administrador> administradores { get; set; }
        public virtual Personalizacion personalizacion { get; set; }
    }
}
