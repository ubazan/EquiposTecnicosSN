using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos.Info
{
    [Table(name: "UMDNS")]
    public class Umdns
    {
        [Key]
        public int UmdnsId { get; set; }

        [Required(ErrorMessage = "El campo Código es requerido.")]
        [DisplayName("Código")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El campo Nombre Completo es requerido.")]
        [DisplayName("Nombre Completo")]
        public string NombreCompleto { get; set; }
    }
}
