using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    [Table("AtributosSesiones")]
    public class AtributoSesion
    {
        public string AdministradorID { get; set; }
        public string TiendaID { get; set; }
        public string AtributoSesionID { get; set; }
        //max 2**31 char
        public string Datos { get; set; }

        [ForeignKey("AdministradorID")]
        public virtual Administrador administrador { get; set; }
        [ForeignKey("TiendaID")]
        public virtual Tienda tienda { get; set; }
    }
}
