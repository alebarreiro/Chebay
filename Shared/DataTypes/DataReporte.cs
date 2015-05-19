using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTypes
{
    public class DataReporte
    {
        public int cantUsuarios { get; set; }
        public int cantTransacciones { get; set; }
        public List<DataReporteUsr> usuarios { get; set; }
        public List<DataReporteTrans> transacciones { get; set; }
    }
}
