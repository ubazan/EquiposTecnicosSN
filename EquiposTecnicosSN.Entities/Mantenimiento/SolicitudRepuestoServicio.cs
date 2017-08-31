using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Mantenimiento
{
    [Table("SolicitudesRepuestosServicios")]
    public class SolicitudRepuestoServicio
    {
        [Key]
        public int SolicitudRepuestoServicioId { get; set; }

        public int OrdenDeTrabajoId { get; set; }

        [ForeignKey("OrdenDeTrabajoId")]
        public virtual OrdenDeTrabajo OrdenDeTrabajo { get; set; }

        [DisplayName("Fecha de inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public DateTime FechaInicio { get; set; }

        [StringLength(500)]
        public string Comentarios { get; set; }

        [DisplayName("Fecha de cierre")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public DateTime? FechaCierre { get; set; }

        public int? ProveedorId { get; set; }

        [DisplayName("Cantidad")]
        public int CantidadRepuesto { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }

        public int? RepuestoId { get; set; }

        [ForeignKey("RepuestoId")]
        public virtual Repuesto Repuesto { get; set; }

        public string UsuarioInicio { get; set; }

        public string UsuarioCierre { get; set; }
    }
}