using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposPruebasDeDiagnostico")]
    public class EquipoPruebaDeDiagnostico : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.PruebasDeDiagnostico;
        }
    }
}