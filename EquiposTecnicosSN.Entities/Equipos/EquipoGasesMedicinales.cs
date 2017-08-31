using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposGasesMedicinales")]
    public class EquipoGasesMedicinales : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.GasesMedicinales;
        }
    }
}
