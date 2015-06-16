using System.Collections.Generic;
namespace Shared.DataTypes
{
    public class DataCalificacionFull
    {
        public List<DataCalificacion> cant1 { get; set; }
        public List<DataCalificacion> cant2 { get; set; }
        public List<DataCalificacion> cant3 { get; set; }
        public List<DataCalificacion> cant4 { get; set; }
        public List<DataCalificacion> cant5 { get; set; }
    }

    public class DataCalificacion
    {
        public string comentario { get; set; }
        public string usuarioEvalua { get; set; }
    }
}
