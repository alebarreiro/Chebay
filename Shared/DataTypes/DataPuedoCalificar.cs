using System;

namespace Shared.DataTypes
{
    public class DataPuedoCalificar
    {
        public bool puedoCalificar { get; set; }
        public string idVendedor { get; set; }
        public string nombreProd { get; set; }
        public int precioProd { get; set; }
        public long idProd { get; set; }
        public DateTime fecha_compra { get; set; }
    }
}

