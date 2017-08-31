namespace EquiposTecnicosSN.Web.Models
{
    public class IndicadoresEquipoViewModel
    {
        public double TiempoIndisponibilidad { get; set; }

        public double TiempoMedioEntreFallas { get; set; }

        public double TiempoMedioReparacion { get; set; }
    }

    public class IndicadoresEquipoPorUbicacionViewModel
    {
        public int EquipoId { get; set; }

        public string NombreEquipo { get; set; }

        public string UbicacionEquipo { get; set; }

        public string SectorEquipo { get; set; }

        public double TiempoIndisponibilidad { get; set; }

        public double TiempoMedioEntreFallas { get; set; }

        public double TiempoMedioReparacion { get; set; }
    }
}