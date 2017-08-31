using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos.Info
{
    [Table("Modelos")]
    public class Modelo
    {
        [Key]
        public int ModeloId { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        [StringLength(150)]
        public string Nombre { get; set; }

        public int MarcaId { get; set; }

        [ForeignKey("MarcaId")]
        public virtual Marca Marca { get; set; }
    }
}