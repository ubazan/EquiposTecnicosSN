using EquiposTecnicosSN.Entities.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EquiposTecnicosSN.Web.Services
{
    public class ODTsService : BaseService
    {

        public IEnumerable<OrdenDeTrabajoMantenimientoCorrectivo> MCorrectivosAbiertos(OrdenDeTrabajoPrioridad? prioridad)
        {
            var mcAbiertos = db.ODTMantenimientosCorrectivos
                .Where(odt => odt.Estado == OrdenDeTrabajoEstado.Abierta || odt.Estado == OrdenDeTrabajoEstado.EsperaRepuesto)
                .Where(odt => prioridad == null || odt.Prioridad == prioridad)
                .OrderBy(odt => odt.FechaInicio);

            return mcAbiertos.ToList();
        }

        public IEnumerable<OrdenDeTrabajoMantenimientoPreventivo> MPreventivosAbiertos(OrdenDeTrabajoPrioridad? prioridad)
        {
            var mpAbiertos = db.ODTMantenimientosPreventivos
                .Where(odt => odt.Estado == OrdenDeTrabajoEstado.Abierta || odt.Estado == OrdenDeTrabajoEstado.EsperaRepuesto)
                .Where(odt => prioridad == null || odt.Prioridad == prioridad)
                .OrderBy(odt => odt.FechaInicio);

            return mpAbiertos.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CorrectivosAbiertosCount()
        {
            return db.ODTMantenimientosCorrectivos
                .Where(odt => odt.Estado == OrdenDeTrabajoEstado.Abierta || odt.Estado == OrdenDeTrabajoEstado.EsperaRepuesto)
                .Count();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int PreventivosAbiertosCount()
        {
            return db.ODTMantenimientosPreventivos
               .Where(odt => odt.Estado == OrdenDeTrabajoEstado.Abierta || odt.Estado == OrdenDeTrabajoEstado.EsperaRepuesto)
               .Count();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<OrdenDeTrabajoMantenimientoPreventivo> MPreventivosProximos()
        {
            var proximos = db.ODTMantenimientosPreventivos
                .Where(odt => odt.FechaInicio >= DateTime.Now)
                .OrderBy(odt => odt.FechaInicio);

            return proximos.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="odtId"></param>
        /// <returns></returns>
        public ICollection<SolicitudRepuestoServicio> BuscarSolicitudes(int? odtId)
        {
            return db.SolicitudesRepuestosServicios.Where(s => s.OrdenDeTrabajoId == odtId).ToList();
        }
        public ICollection<GastoOrdenDeTrabajo> BuscarGastos(int ordenDeTrabajoId)
        {
            return db.GastosOrdenesDeTrabajo.Where(g => g.OrdenDeTrabajoId == ordenDeTrabajoId).ToList();
        }
    }
}