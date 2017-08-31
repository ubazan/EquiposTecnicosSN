using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquiposTecnicosSN.Entities.Mantenimiento
{
    [Table("GastosOrdenesDeTrabajo")]
    public class GastoOrdenDeTrabajo
    {
        [Key]
        public int GastoOrdenDeTrabajoId { get; set; }

        [Required]
        public int OrdenDeTrabajoId { get; set; }

        [ForeignKey("OrdenDeTrabajoId")]
        public virtual OrdenDeTrabajo OrdenDeTrabajo { get; set; }

        [Required]
        [StringLength(100)]
        public string Concepto { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public Decimal Monto { get; set; }

        public int? SolicitudRepuestoServicioId { get; set; }

        [ForeignKey("SolicitudRepuestoServicioId")]
        public virtual SolicitudRepuestoServicio SolicitudRepuestoServicio { get; set; }

        public int? GastoProveedorId { get; set; }

        [ForeignKey("GastoProveedorId")]
        public virtual Proveedor proveedor { get; set; }

    }
}
