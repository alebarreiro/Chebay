using System;		
using System.Collections.Generic;		
using System.Linq;		
using System.Text;		
using System.Threading.Tasks;		
		
namespace Shared.DataTypes		
{		
    public class DataProducto		
    {		
        public string nombre { get; set; }		
        public string descripcion { get; set; }		
        public int precio_base_subasta { get; set; }		
        public int precio_compra { get; set; }		
        public DateTime fecha_cierre { get; set; }		
    }		
}