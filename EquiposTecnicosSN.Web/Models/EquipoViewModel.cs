using EquiposTecnicosSN.Entities.Equipos;
using EquiposTecnicosSN.Entities.Equipos.Info;
using EquiposTecnicosSN.Entities.Mantenimiento;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EquiposTecnicosSN.Web.Models
{
    public class EquipoViewModel
    {
        // Solamente usados en EquipoClimatizacionViewModel
        public IEnumerable<SelectListItem> Ubicaciones { get; set; }
        public int UbicacionSelectedId { get; set; }

        // Generales
        public InformacionComercial InformacionComercial { get; set; }
    }

    public class EquipocClimatizacionViewModel : EquipoViewModel
    {
        public EquipoClimatizacion Equipo { get; set; }
        public IEnumerable<SelectListItem> UbicacionId { get; set; }
    }

    public class HomeViewModel
    {
        public SearchEquipoViewModel searchEquipo { get; set; }
        
        public SearchOdtViewModel searchOdt { get; set; }

        public int CorrectivosCount { get; set; }

        public int PreventivosCount { get; set; }

        public int EquiposFuncionalesCount { get; set; }

        public int RepuestosCount { get; set; }
    }

    public class SearchEquipoViewModel
    {
        [DisplayName("Nº de matrícula")]
        public int? NumeroMatricula { get; set; }

        public virtual InformacionHardware NumeroSerie { get; set; }

        public int UbicacionId { get; set; }

        [DisplayName("Ubicación")]
        public virtual Ubicacion Ubicacion { get; set; }

        public int SectorId { get; set; }

        public virtual Sector Sector { get; set; }

        public EstadoDeEquipo EstadoEquipo { get; set; }

        public TipoEquipo SearchTipoEquipo { get; set; }

        public int PageSize { get; set; }
    }



}