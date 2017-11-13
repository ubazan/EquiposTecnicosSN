using EquiposTecnicosSN.Entities.Equipos;
using EquiposTecnicosSN.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Clase que representa una orden de trabajo base.
/// </summary>
namespace EquiposTecnicosSN.Entities.Mantenimiento
{
    [Table("OrdenesDeTrabajo")]
    public abstract class OrdenDeTrabajo
    {
        [DisplayName("Nro ODT")]
        [Key]
        [Required]
        public int OrdenDeTrabajoId { get; set; }

        [Required]
        public int EquipoId { get; set; }

        [ForeignKey("EquipoId")]
        [Column("Equipo")]
        public virtual Equipo Equipo { get; set; }

        [DisplayName("Fecha de incio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, NullDisplayText = " ")]
        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public string UsuarioInicio { get; set; }

        public virtual ICollection<GastoOrdenDeTrabajo> Gastos { get; set; }

        public virtual ICollection<SolicitudRepuestoServicio> SolicitudesRespuestos { get; set; }

        public virtual ICollection<ObservacionOrdenDeTrabajo> Observaciones { get; set; }

        [DisplayName("Fecha de cierre")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public DateTime? FechaCierre { get; set; }

        public string UsuarioCierre { get; set; }

        public OrdenDeTrabajoPrioridad Prioridad { get; set; }

        public OrdenDeTrabajoEstado Estado { get; set; }

    }
}
