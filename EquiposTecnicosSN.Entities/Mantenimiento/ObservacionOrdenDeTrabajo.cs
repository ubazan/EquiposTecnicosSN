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
    [Table("ObservacionesOrdenDeTrabajo")]
    public class ObservacionOrdenDeTrabajo
    {
        [Key]
        public int ObservacionOrdenDeTrabajoId { get; set; }

        public int OrdenDeTrabajoId { get; set; }

        [ForeignKey("OrdenDeTrabajoId")]
        public virtual OrdenDeTrabajo OrdenDeTrabajo { get; set; }

        [StringLength(500)]
        public string Observacion { get; set; }

        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public DateTime Fecha { get; set; }

        public string Usuario { get; set; }
    }
}
