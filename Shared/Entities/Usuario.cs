using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Usuarios")]
    public class Usuario
    {
        public string UsuarioID { get; set; }
        public string token { get; set; }
        //add twitter or fb tokens
        public int compras_valor { get; set; }
        public int ventas_valor { get; set; }

        public virtual ICollection<Producto> publicados { get; set; }
        public virtual ICollection<Visita> visitas { get; set; }
        public virtual ICollection<Favorito> favoritos { get; set; }
        public virtual ICollection<Oferta> ofertas { get; set; }
        public virtual ICollection<Compra> compras { get; set; }
        public virtual ICollection<Comentario> comentarios { get; set; }

    }
}
