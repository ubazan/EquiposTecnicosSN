using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposEndoscopia")]
    public class EquipoEndoscopia : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.Endoscopia;
        }
    }
}
