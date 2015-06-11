using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities
{
    [Table("Usuarios")]
    public class Usuario
    {
        public string UsuarioID { get; set; }

        //opcionales...
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string NumeroContacto { get; set; }
        public int CodigoPostal { get; set; }
        public string Email { get; set; }

        public int compras_valor { get; set; }
        public int ventas_valor { get; set; }
        public DateTime fecha_ingreso { get; set; }
        public double promedio_calificacion { get; set; }

        public virtual ICollection<Producto> publicados { get; set; }
        public virtual ICollection<Producto> visitas { get; set; }
        public virtual ICollection<Producto> favoritos { get; set; }
        public virtual ICollection<Oferta> ofertas { get; set; }
        public virtual ICollection<Compra> compras { get; set; }
        public virtual ICollection<Comentario> comentarios { get; set; }
        //propias y realizadas
        public virtual ICollection<Calificacion> calificaciones {get; set;}
        public virtual ICollection<Calificacion> calificacionesrecibidas { get; set; }

        public virtual ImagenUsuario Imagen { get; set; }
    }
}
