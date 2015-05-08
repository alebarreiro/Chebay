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
        [Key]
        public string AtributoSesionID { get; set; }
        public string AdministradorID { get; set; }
        //max 2**31 char
        public string Datos { get; set; }

        [ForeignKey("AdministradorID")]
        public virtual Administrador administrador { get; set; }

    }
}
