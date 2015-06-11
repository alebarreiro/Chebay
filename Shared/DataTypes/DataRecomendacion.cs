using System.Collections.Generic;

namespace Shared.DataTypes
{
    public class DataRecomendacion
    {
        public string UsuarioID { get; set; }
        public List<DataProducto> productos { get; set; }
    }
}
