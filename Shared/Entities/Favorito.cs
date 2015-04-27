using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Favoritos")]
    public class Favorito
    {
        public long FavoritoID { get; set; }
        public string UsuarioID { get; set; }
        public string ProductoID { get; set; }
        public virtual Producto producto { get; set; }
        public virtual Usuario usuario { get; set; }
    }
}
