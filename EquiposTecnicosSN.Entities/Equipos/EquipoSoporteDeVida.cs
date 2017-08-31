using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposSoporteDeVida")]
    public class EquipoSoporteDeVida : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.SoporteDeVida;
        }
    }
}
