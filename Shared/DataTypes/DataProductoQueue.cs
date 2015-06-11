using System;

namespace Shared.DataTypes
{
    public class DataProductoQueue
    {
        public string TiendaID { get; set; }
        public long ProductoID { get; set; }
        public string OwnerProducto { get; set; }
        public string nombre { get; set; }
        public DateTime fecha_cierre { get; set; }
    }
}
