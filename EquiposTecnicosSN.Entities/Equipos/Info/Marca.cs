using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos.Info
{
    [Table("Marcas")]
    public class Marca
    {
        [Key]
        public int MarcaId { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        [StringLength(150)]
        public string Nombre { get; set; }

        public int FabricanteId { get; set; }

        [ForeignKey("FabricanteId")]
        public virtual Fabricante Fabricante { get; set; }

        public virtual ICollection<Modelo> Modelos { get; set; }
    }
}