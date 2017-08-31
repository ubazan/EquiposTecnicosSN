using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Mantenimiento
{
    [Table("Proveedores")]
    public class Proveedor
    {
        [Key]
        [Required]
        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Tipo es requerido.")]
        public ProveedorTipo Tipo { get; set; }

        [DisplayName("Dirección")]
        [StringLength(255)]
        public string Direccion { get; set; }

        [DisplayName("Teléfono")]
        [StringLength(100)]
        public string Telefono { get; set; }

        [StringLength(255)]
        public string Website { get; set; }

        [DisplayName("Email")]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(255)]
        public string Servicios { get; set; }
    }
}
