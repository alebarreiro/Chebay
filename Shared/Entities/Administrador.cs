using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Administradores")]
    public class Administrador
    {
        //id?
        public string TiendaID { get; set; }
        public string AdministradorID { get; set; }
        public string password { get; set; }
        //public virtual Tienda tienda { get; set; }
    }
}
