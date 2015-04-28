using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Productos")]
    public class Producto
    {
        public long ProductoID { get; set; }
        public string UsuarioID { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int precio_base_subasta {get; set;}
        public int precio_compra { get; set; }
        public DateTime fecha_cierre { get; set; }

        //[ForeignKey("categoria")]
        public string CategoriaID { get; set; }

        public virtual Usuario usuario { get; set; }
        public virtual ICollection<Visita> visitas { get; set; }
        public virtual ICollection<Favorito> favoritos { get; set; }
        public virtual ICollection<Oferta> ofertas { get; set; }
        public virtual ICollection<Compra> compras { get; set; } //tiene una única compra.
        public virtual ICollection<Comentario> comentarios { get; set; } //nuevo

        //se deducen los atributos
        public virtual CategoriaSimple categoria { get; set; }
    }
}
