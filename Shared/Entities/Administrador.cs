using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Administradores")]
    public class Administrador
    {
        [Key]
        public string AdministradorID { get; set; }
        [Required]
        public string password { get; set; }
        
        public virtual ICollection<Tienda> tiendas { get; set; }
        public virtual ICollection<AtributoSesion> atributosSesion { get; set; }

    }
}
