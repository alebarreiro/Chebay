using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace ChebayREST.Models
{
    public class Subasta
    {
        public long ProductoID { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha_cierre { get; set; }
        public int precio_actual { get; set; }
        public string idOfertante { get; set; }
    }

}