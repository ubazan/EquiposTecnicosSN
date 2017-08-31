using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposInformatica")]
    public class EquipoInformatica : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.Informatica;
        }
    }
}