using System.ComponentModel.DataAnnotations;

namespace EquiposTecnicosSN.Entities.Mantenimiento
{
    public enum OrdenDeTrabajoEstado
    {
        Abierta = 1,
        [Display(Name ="A la espera de repuestos")]
        EsperaRepuesto = 2,
        Cerrada = 3,
        Cancelada = 4
    }
}