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
    [Table("StockRepuestos")]
    public class StockRepuesto
    {
        [Key]
        public int StockRepuestoId { get; set; }

        [DisplayName("Cantidad disponible")]
        [Required(ErrorMessage = "El campo Cantidad Disponible es requerido.")]
        public int CantidadDisponible { get; set; }

        [ForeignKey("Repuesto")]
        public int RepuestoId { get; set; }
        
        public virtual Repuesto Repuesto { get; set; }

    }
}
