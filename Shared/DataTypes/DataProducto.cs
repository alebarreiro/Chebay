using MongoDB.Bson.Serialization.Attributes;
using Shared.Entities;
using System;

namespace Shared.DataTypes		
{		
    public class DataProducto		
    {
        public DataProducto()
        {

        }

        public DataProducto(Producto p)
        {            
            ProductoID = p.ProductoID;
            nombre = p.nombre;
            descripcion = p.descripcion;
            precio_base_subasta = p.precio_base_subasta;
            precio_compra = p.precio_compra;
            fecha_cierre = p.fecha_cierre.ToUniversalTime();
            idOfertante = p.UsuarioID;
        }
  
        public long ProductoID { get; set; }
        public string nombre { get; set; }		
        public string descripcion { get; set; }		
        public int precio_base_subasta { get; set; }		
        public int precio_compra { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime fecha_cierre { get; set; }
        public int precio_actual { get; set; }
        public string idOfertante { get; set; }
    }		
}