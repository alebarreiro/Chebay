using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    [Table("Personalizaciones")]
    public class Personalizacion
    {
        [Key, ForeignKey("tienda")]
        public string PersonalizacionID { get; set; }
        public string datos { get; set; }
        public byte[] algoritmo { get; set; }
        public virtual Tienda tienda { get; set; }
    }
}
