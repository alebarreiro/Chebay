using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataTypes
{
    public class DataRecomendacion
    {
       // public Guid Id { get; set; }
        public string UsuarioID { get; set; }
        public List<DataProducto> productos { get; set; }
    }
}
