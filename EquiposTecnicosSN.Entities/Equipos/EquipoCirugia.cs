using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposCirugia")]
    public class EquipoCirugia : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.Cirugia;
        }
    }
}