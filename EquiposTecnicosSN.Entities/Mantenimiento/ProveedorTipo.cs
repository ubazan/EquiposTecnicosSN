using System.ComponentModel.DataAnnotations;

namespace EquiposTecnicosSN.Entities.Mantenimiento
{
    public enum ProveedorTipo
    {
        Fabricante = 0,
        Proveedor = 1,
        [Display(Name ="Fabricante y Proveedor")]
        FabricanteYProveedor = 2,
        Otro = 3
    }
}