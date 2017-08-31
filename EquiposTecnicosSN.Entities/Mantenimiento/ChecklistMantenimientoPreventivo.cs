using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquiposTecnicosSN.Entities.Mantenimiento
{
    [Table("ChecklistsMantenimientoPreventivo")]
    public class ChecklistMantenimientoPreventivo
    {
        [Key]
        [Required]
        public int ChecklistMantenimientoPreventivoId { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Nombre { get; set; }

        [StringLength(100)]
        [Required]
        public string ContentType { get; set; }

        [Required]
        public byte[] Content { get; set; }

        [Required]
        [StringLength(5)]
        public string FileExtension { get; set; }
    }
}
