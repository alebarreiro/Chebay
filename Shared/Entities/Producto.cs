using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Productos")]
    public class Producto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductoID { get; set; }
        public string UsuarioID { get; set; }
        [Required]
        public string nombre { get; set; }
        public string descripcion { get; set; }
        [Required]
        public int precio_base_subasta {get; set;}
        [Required]
        public int precio_compra { get; set; }
        [Required]
        public DateTime fecha_cierre { get; set; }

        public string latitud { get; set; }
        public string longitud { get; set; }

        public long CategoriaID { get; set; }

        [ForeignKey("UsuarioID")]
        public virtual Usuario usuario { get; set; }
        [ForeignKey("CategoriaID")]
        public virtual CategoriaSimple categoria { get; set; }
        
        public virtual Compra compra { get; set; }

        public virtual ICollection<Usuario> visitas { get; set; }
        public virtual ICollection<Usuario> favoritos { get; set; }
        public virtual ICollection<Oferta> ofertas { get; set; }
        public virtual ICollection<Comentario> comentarios { get; set; }
        public virtual ICollection<Atributo> atributos { get; set; }
        public virtual ICollection<ImagenProducto> imagenes { get; set; }
        
    }
}
