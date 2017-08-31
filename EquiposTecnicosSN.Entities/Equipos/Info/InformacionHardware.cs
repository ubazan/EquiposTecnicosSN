using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos.Info
{
    [Table("InformacionHardware")]
    public class InformacionHardware
    {
        [Key]
        [ForeignKey("Equipo")]
        [Required]
        public int EquipoId { get; set; }

        public virtual Equipo Equipo { get; set; }

        [StringLength(255)]
        [DisplayName("Nº de serie")]
        [Required(ErrorMessage = "El campo Nº de Serie es requerido.")]
        public string NumeroSerie { get; set; }

        [Required(ErrorMessage = "El campo Fabricante es requerido.")]
        public int FabricanteId { get; set; }

        [ForeignKey("FabricanteId")]
        public virtual Fabricante Fabricante { get; set; }

        [Required(ErrorMessage = "El campo Marca es requerido.")]
        public int MarcaId { get; set; }

        [ForeignKey("MarcaId")]
        public virtual Marca Marca { get; set; }

        [Required(ErrorMessage = "El campo Modelo es requerido.")]
        public int ModeloId { get; set; }

        [ForeignKey("ModeloId")]
        public virtual Modelo Modelo { get; set; }

        [DisplayName("Año Fabricación")]
        public int? AnioFabricacion { get; set; }
    }
}
