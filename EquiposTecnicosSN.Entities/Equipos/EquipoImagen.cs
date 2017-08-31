using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Equipos
{
    [Table("EquiposImagenes")]
    public class EquipoImagen : Equipo
    {
        public override TipoEquipo Tipo()
        {
            return TipoEquipo.Imagenes;
        }
    }
}