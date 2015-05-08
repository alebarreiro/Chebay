using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
