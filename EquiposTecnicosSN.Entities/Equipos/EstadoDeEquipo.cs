using System.ComponentModel.DataAnnotations;

namespace EquiposTecnicosSN.Entities.Equipos
{
    public enum EstadoDeEquipo
    {
        [Display(Name = "Operativo")]
        Funcional = 1,
        [Display(Name="Fuera de servicio")]
        NoFuncional = 2,
        [Display(Name = "Operativo condicional")]
        FuncionalRequiereReparacion = 3,
        [Display(Name = "Fuera de servicio")]
        NoFuncionalRequiereReparacion = 4,
        [Display(Name = "Baja de equipo")]
        Baja = 5
    }
}
