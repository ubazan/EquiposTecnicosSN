using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/// <summary>
/// Clase que representa una orden de trabajo para mantenimiento preventivo.
/// </summary>
namespace EquiposTecnicosSN.Entities.Mantenimiento
{
    [Table("OrdenesDeTrabajoMantenimientoPreventivo")]
    public class OrdenDeTrabajoMantenimientoPreventivo : OrdenDeTrabajo
    {
        [Required]
        public int ChecklistId { get; set; }

        [ForeignKey("ChecklistId")]
        public virtual ChecklistMantenimientoPreventivo Checklist { get; set; }

        [DisplayName("Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public DateTime fechaCreacion { get; set; }

        [Required]
        public string UsuarioCreacion { get; set; }

        [DisplayName("Checklist Completo")]
        public bool ChecklistCompleto { get; set; }

        [StringLength(100)]
        public string ChecklistCompletoContentType { get; set; }

        public byte[] ChecklistCompletoContent { get; set; }

        [StringLength(5)]
        public string ChecklistCompletoFileExtension { get; set; }

    }
}
