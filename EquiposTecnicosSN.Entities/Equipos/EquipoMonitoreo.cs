using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposMonitoreo")]
    public class EquipoMonitoreo : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.Monitoreo;
        }
    }
}