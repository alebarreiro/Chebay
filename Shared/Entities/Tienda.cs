using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared.Entities
{
    [Table("Tiendas")]
    public class Tienda
    {
        //url
        public string TiendaID { get; set; }
        [Required]
        public string nombre { get; set; }
        public string descripcion { get; set; }

        public virtual ICollection<Administrador> administradores { get; set; }
    }
}
