using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposLuces")]
    public class EquipoLuces : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.Luces;
        }
    }
}