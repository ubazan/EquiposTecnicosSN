using EquiposTecnicosSN.Entities.Equipos.Info;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Usuarios
{
    [Table("SolicitudesUsuario")]
    public class SolicitudUsuario
    {
        [Key]
        public int SolicitudUsuarioId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DisplayName("Ubicación")]
        public int UbicacionId { get; set; }

        public virtual Ubicacion Ubicacion { get; set; }
    }
}
