using EquiposTecnicosSN.Entities.Mantenimiento;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Web;
using EquiposTecnicosSN.Entities.Equipos.Info;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Web.Models
{
    public class SearchOdtViewModel
    {
        public OrdenDeTrabajoEstado EstadoODT { get; set; }

        public OrdenDeTrabajoTipo TipoODT { get; set; }

        public DateTime? FechaInicio { get; set; }
    }

    public class NewOrdenDeTrabajoViewModel
    {
        public OrdenDeTrabajoMantenimientoCorrectivo NewOrden { get; set; }

        public ICollection<GastoOrdenDeTrabajo> Gastos { get; set; }
    }

    public abstract class MViewModel
    {
        [DisplayName("Nueva Observación")]
        public ObservacionOrdenDeTrabajo NuevaObservacion { get; set; }

        /// <summary> Jose Gutierrez -- 30/05/2017
        /// Se agrego para el post ya que en el controlador ODTMantenimientoCorrectivoController no obtiene el proveedorId 
        /// que necesita para almacenarlo en la base de datos y asociarle a la orden de trabajo
        /// </summary>
        public int? ProveedorId { get; set; }

        [DisplayName("Proveedor")]
        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }
    }


    public class MCViewModel : MViewModel
    {
        public OrdenDeTrabajoMantenimientoCorrectivo Odt { get; set; }
    }

    public class MPViewModel : MViewModel
    {
        public OrdenDeTrabajoMantenimientoPreventivo Odt { get; set; }

        public HttpPostedFileBase ChecklistCompletoFile { get; set; }
    }

    public class MCIndexViewModel
    {
        public SearchOdtViewModel Search { get; set; }

        public IEnumerable<OrdenDeTrabajoMantenimientoCorrectivo> Emergencias { get; set; }

        public IEnumerable<OrdenDeTrabajoMantenimientoCorrectivo> Urgencias { get; set; }

        public IEnumerable<OrdenDeTrabajoMantenimientoCorrectivo> Normales { get; set; }
    }

    public class MPIndexViewModel
    {
        public SearchOdtViewModel Search { get; set; }

        public IEnumerable<OrdenDeTrabajoMantenimientoPreventivo> Emergencias { get; set; }

        public IEnumerable<OrdenDeTrabajoMantenimientoPreventivo> Urgencias { get; set; }

        public IEnumerable<OrdenDeTrabajoMantenimientoPreventivo> Normales { get; set; }

        public IEnumerable<OrdenDeTrabajoMantenimientoPreventivo> Proximas { get; set; }

        public IEnumerable<OrdenDeTrabajoMantenimientoPreventivo> Abiertas { get; set; }
    }

    public class ODTIndexModel
    {
        public MCIndexViewModel mcViewModel { get; set; }

        public MPIndexViewModel mpViewModel { get; set; }
    }
}