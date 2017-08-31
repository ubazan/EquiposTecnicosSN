using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposTerapeutica")]
    public class EquipoTerapeutica : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.Terapeutica;
        }
    }
}