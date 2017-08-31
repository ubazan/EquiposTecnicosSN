using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquiposTecnicosSN.Entities.Equipos.Info
{
    [Table("Fabricantes")]
    public class Fabricante
    {
        [Key]
        public int FabricanteId { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        [StringLength(150)]
        public string Nombre { get; set; }

        public virtual ICollection<Marca> Marcas { get; set; }
    }
}
