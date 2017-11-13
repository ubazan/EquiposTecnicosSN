using EquiposTecnicosSN.Entities.Mantenimiento;
using EquiposTecnicosSN.Web.DataContexts;
using EquiposTecnicosSN.Web.Models;
using EquiposTecnicosSN.Web.Services;
using PagedList;
using Salud.Security.SSO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EquiposTecnicosSN.Web.Controllers
{
    /// <summary>
    /// Web Controller Base para ordenes de trabajo
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public abstract class ODTController : Controller
    {
        /// <summary>
        /// DbContext de equipo
        /// </summary>
        protected EquiposDbContext db = new EquiposDbContext();
        /// <summary>
        /// 
        /// </summary>
        protected ODTsService odtsService = new ODTsService();
        /// <summary>
        /// The equipos service
        /// </summary>
        protected EquiposService equiposService = new EquiposService();

        /// <summary>
        /// Acción que carga los datos de una orden de trabajo.
        /// </summary>
        /// <param name="id">Id de la orden de trabajo</param>
        /// <returns></returns>
        abstract public ActionResult Details(int? id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public ActionResult CreateForEquipo(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public ActionResult EditGastos(int id);

        /// <summary>
        /// Guarda la fecha en que se cerro, el usuario que lo cerro y cambia el estado de la OT en Cerrada 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public ActionResult Close(int id);


        /// <summary>
        /// Fue nombrado a reparar, guarda los detalles de la reparacion y deja en estado reparado la OT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public ActionResult Reparar(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddGasto()
        {
            return PartialView("_AddGastoToOrden", new GastoOrdenDeTrabajo());
        }

        /// <summary>
        /// Guarda los gastos asociados a una orden de trabajo.
        /// </summary>
        /// <param name="gastos">Lista de gastos a presistir</param>
        /// <param name="ordenDeTrabajoId">Id de la orden de trabajo</param>
        protected void SaveGastos(IEnumerable<GastoOrdenDeTrabajo> gastos, int ordenDeTrabajoId)
        {
            var gastosEntidad = db.GastosOrdenesDeTrabajo.Where(g => g.OrdenDeTrabajoId == ordenDeTrabajoId).ToList();

            foreach (var gastoE in gastosEntidad)
            {
                if (gastos.Any(g => g.GastoOrdenDeTrabajoId == gastoE.GastoOrdenDeTrabajoId))
                {
                    var edicion = gastos.Where(g => g.GastoOrdenDeTrabajoId == gastoE.GastoOrdenDeTrabajoId).Single();
                    gastoE.Monto = edicion.Monto;
                    gastoE.Concepto = edicion.Concepto;
                    gastoE.GastoProveedorId = edicion.GastoProveedorId;
                    db.Entry(gastoE).State = EntityState.Modified;
                }
                else
                {
                    db.GastosOrdenesDeTrabajo.Remove(gastoE);
                }
            }

            if (gastos != null)
            {
                var nuevosGastos = gastos.Where(g => g.GastoOrdenDeTrabajoId == 0);
                if (nuevosGastos != null && nuevosGastos.Count() > 0)
                {
                    nuevosGastos.ToList().ForEach(g =>
                    {
                        g.OrdenDeTrabajoId = ordenDeTrabajoId;
                        db.GastosOrdenesDeTrabajo.Add(g);
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nuevaObservacion"></param>
        /// <param name="odt"></param>
        protected void SaveNuevaObservacion(ObservacionOrdenDeTrabajo nuevaObservacion, OrdenDeTrabajo odt)
        {
            if (nuevaObservacion.Observacion != null)
            {
                if (odt.Observaciones == null)
                {
                    odt.Observaciones = new List<ObservacionOrdenDeTrabajo>();
                }

                odt.Observaciones.Add(nuevaObservacion);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orden"></param>
        public void CloseSolicitudesRepuestos(OrdenDeTrabajo orden)
        {
            foreach (var s in orden.SolicitudesRespuestos)
            {
                if (s.FechaCierre == null)
                {
                    s.FechaCierre = DateTime.Now;
                    db.Entry(s).State = EntityState.Modified;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected ObservacionOrdenDeTrabajo NuevaObservacion()
        {
            return new ObservacionOrdenDeTrabajo
            {
                Fecha = DateTime.Now,
                Usuario = (SSOHelper.CurrentIdentity != null ? SSOHelper.CurrentIdentity.Fullname : "Usuario Anónimo")
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buscarNumeroReferencia"></param>
        /// <param name="EstadoODT"></param>
        /// <returns></returns>
        public ActionResult SearchODT(string FechaInicio = "", int? buscarNumeroReferencia = 0, int? EstadoODT = 0, int? TipoODT = 0, int page = 1, int pageSize = 25)
        {

            //var query = db.ODTMantenimientosCorrectivos
            //    .Where(odt => buscarNumeroReferencia == 0 || odt.OrdenDeTrabajoId == buscarNumeroReferencia);
            

            var result = db.OrdenesDeTrabajo
                .Where(odt => buscarNumeroReferencia == 0 || odt.OrdenDeTrabajoId == buscarNumeroReferencia);

            if (FechaInicio != "")
            {
                var fecha = DateTime.Parse(FechaInicio);
                result = result.Where(odt => DateTime.Compare(odt.FechaInicio, fecha) >= 0);

            }

            if (EstadoODT != 0)
            {
                OrdenDeTrabajoEstado estadoFiltro = (OrdenDeTrabajoEstado)EstadoODT;
                result = result.Where(odt => odt.Estado == estadoFiltro);
            }

            if (TipoODT != 0)
            {
                OrdenDeTrabajoTipo tipoFiltro = (OrdenDeTrabajoTipo)TipoODT;

                switch (tipoFiltro)
                {
                    case OrdenDeTrabajoTipo.Correctivo:
                        result = result.Where(odt => odt is OrdenDeTrabajoMantenimientoCorrectivo);                        
                        break;

                    case OrdenDeTrabajoTipo.Preventivo:
                        result = result.Where(odt => odt is OrdenDeTrabajoMantenimientoPreventivo);
                        break;
                }
            }

            return PartialView("_SearchODTsResults", result.OrderByDescending(odt => odt.FechaInicio).ToPagedList(page, pageSize));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private MPIndexViewModel IndexMP()
        {
            var model = new MPIndexViewModel
            {
                Search = new SearchOdtViewModel(),
                Proximas = odtsService.MPreventivosProximos(),
                Abiertas = odtsService.MPreventivosAbiertos(null)
            };

            return model;
        }

        /// <summary>
        /// Action Index
        /// </summary>
        /// <returns></returns>
        private MCIndexViewModel IndexMC()
        {
            var model = new MCIndexViewModel
            {
                Emergencias = odtsService.MCorrectivosAbiertos(OrdenDeTrabajoPrioridad.Emergencia),
                Urgencias = odtsService.MCorrectivosAbiertos(OrdenDeTrabajoPrioridad.Urgencia),
                Normales = odtsService.MCorrectivosAbiertos(OrdenDeTrabajoPrioridad.Normal),
                Search = new SearchOdtViewModel()
            };

            return model;
        }

        public ActionResult IndexMantenimientos()
        {

            var model = new ODTIndexModel
            {
                mcViewModel = IndexMC(),
                mpViewModel = IndexMP()
            };
            return View(model);
        }

    }
}
