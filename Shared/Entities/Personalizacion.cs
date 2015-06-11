using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Personalizaciones")]
    public class Personalizacion
    {
        [Key, ForeignKey("tienda")]
        public string PersonalizacionID { get; set; }
        public string datos { get; set; }
        public byte[] algoritmo { get; set; }
        public int template { get; set; }
        public byte[] backgroud_image { get; set; }
        public string css { get; set; }

        public virtual Tienda tienda { get; set; }
    }
}
