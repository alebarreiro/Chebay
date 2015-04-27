using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Tiendas")]
    public class Tienda
    {
        //url
        public string TiendaID { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

        //public virtual ICollection<Usuario> usuarios { get; set; }
        public virtual ICollection<Administrador> administradores { get; set; }
    }
}
