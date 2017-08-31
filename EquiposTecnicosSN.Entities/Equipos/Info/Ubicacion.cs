using EquiposTecnicosSN.Entities.Equipos;
using EquiposTecnicosSN.Entities.Mantenimiento;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos.Info
{
    [Table("Ubicaciones")]
    public class Ubicacion
    {
        [Key]
        [Required]
        public int UbicacionId { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        [StringLength(150)]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        public virtual ICollection<Equipo> EquiposTecnicos { get; set; }

        public Ubicacion()
        {
            EquiposTecnicos = new LinkedList<Equipo>();
        }
    }
}
