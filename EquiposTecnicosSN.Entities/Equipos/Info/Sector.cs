using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquiposTecnicosSN.Entities.Equipos.Info
{
    [Table("Sectores")]
    public class Sector
    {
        [Key]
        [Required]
        public int SectorId { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        [StringLength(150)]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        public virtual ICollection<Equipo> EquiposTecnicos { get; set; }

        public Sector()
        {
            EquiposTecnicos = new LinkedList<Equipo>();
        }
    }
}
