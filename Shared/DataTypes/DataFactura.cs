using System;

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
