using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposOdontologia")]
    public class EquipoOdontologia : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.Odontologia;
        }
    }
}