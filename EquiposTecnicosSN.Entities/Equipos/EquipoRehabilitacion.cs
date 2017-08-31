using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposRehabilitacion")]
    public class EquipoRehabilitacion : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.Rehabilitacion;
        }
    }
}