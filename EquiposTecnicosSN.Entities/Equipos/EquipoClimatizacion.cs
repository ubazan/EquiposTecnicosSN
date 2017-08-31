using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposClimatizacion")]
    public class EquipoClimatizacion : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.Climatizacion;
        }
    }
}