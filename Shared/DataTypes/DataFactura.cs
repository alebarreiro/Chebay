using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTypes
{
    public class DataFactura
    {
        public string nombreProducto { get; set; }
        public long ProductoID { get; set; }
        public bool esCompra { get; set; }
        public int monto { get; set; }
        public DateTime fecha { get; set; }
    }
}
