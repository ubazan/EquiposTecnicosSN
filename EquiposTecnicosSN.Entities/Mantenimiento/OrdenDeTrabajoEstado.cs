using System.ComponentModel.DataAnnotations;

namespace EquiposTecnicosSN.Entities.Mantenimiento
{
    public enum OrdenDeTrabajoEstado
    {
        Abierta = 1,
        [Display(Name ="A la espera de repuestos")]
        EsperaRepuesto = 2,
        Reparada = 3,
        Cerrada = 4,
        Cancelada = 5
    }
}