using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("AtributosSesiones")]
    public class AtributoSesion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string AdministradorID { get; set; }
        public string AtributoSesionID { get; set; }
        public string Datos { get; set; }

        [ForeignKey("AdministradorID")]
        public virtual Administrador administrador { get; set; }

    }
}
