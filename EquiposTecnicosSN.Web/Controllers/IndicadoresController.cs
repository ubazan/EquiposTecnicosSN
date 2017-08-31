using EquiposTecnicosSN.Web.DataContexts;
using EquiposTecnicosSN.Web.Models;
using EquiposTecnicosSN.Web.Services;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EquiposTecnicosSN.Web.Controllers
{
    /// <summary>
    /// Web Controller de las pantallas de indicadores.
    /// </summary>
    public class IndicadoresController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();
        private IndicadoresService indicadoresSrv = new IndicadoresService();

        /// <summary>
        /// Acción Web PorUbicacion.
        /// </summary>
        /// <returns>View Model</returns>
        public ActionResult PorUbicacion()
        {
            ViewBag.UbicacionId = new SelectList(db.Ubicaciones.OrderBy(u => u.Nombre), "UbicacionId", "Nombre");
            ViewBag.SectorId = new SelectList(db.Sectores.OrderBy(u => u.Nombre), "SectorId", "Nombre");
            return View();
        }

        /// <summary>
        /// Acción Web PorUMDNS.
        /// </summary>
        /// <returns></returns>
        public ActionResult PorUMDNS()
        {
            ViewBag.UbicacionId = new SelectList(db.Ubicaciones.OrderBy(u => u.Nombre), "UbicacionId", "Nombre");
            return View();
        }

        /// <summary>
        /// Devuelve una tabla con los indicadores de los equipos que resulten de la consulta.
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="SectorId"></param>
        /// <param name="UbicacionId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult IndicadoresPorUbicacion(string fechaInicio, string fechaFin, int SectorId = 0, int UbicacionId = 0, int page = 1)
        {
            var fechaInicioDT = DateTime.Parse(fechaInicio);
            var fechaFinDT = DateTime.Parse(fechaFin);

            var modelList = db.Equipos
                .Where(e => UbicacionId == 0 || e.UbicacionId == UbicacionId)
                .Where(e => SectorId == 0 || e.SectorId == SectorId)
                .OrderBy(e => e.Ubicacion.Nombre).OrderBy(e => e.Sector.Nombre)
                .Select(e => new IndicadoresEquipoPorUbicacionViewModel
                {
                    EquipoId = e.EquipoId,
                    NombreEquipo = e.NombreCompleto,
                    UbicacionEquipo = e.Ubicacion.Nombre,
                    SectorEquipo = e.Sector.Nombre
                })
                .ToPagedList(page, 10);

            CalcularIndicadoresParaEquipos(fechaInicioDT, fechaFinDT, modelList);

            return PartialView("_IndicadoresPorUbicacionTable", modelList);
        }

        /// <summary>
        /// Devuelve una tabla con los indicadores de los equipos que resulten de la consulta.
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="SectorId"></param>
        /// <param name="UbicacionId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult IndicadoresPorUMDNS(string fechaInicio, string fechaFin, string umdns, int UbicacionId = 0, int page = 1)
        {
            var fechaInicioDT = DateTime.Parse(fechaInicio);
            var fechaFinDT = DateTime.Parse(fechaFin);

            var modelList = db.Equipos
                .Where(e => UbicacionId == 0 || e.UbicacionId == UbicacionId)
                .Where(e => e.UMDNS == umdns)
                .OrderBy(e => e.UMDNS)
                .Select(e => new IndicadoresEquipoPorUbicacionViewModel
                {
                    EquipoId = e.EquipoId,
                    NombreEquipo = e.NombreCompleto,
                    UbicacionEquipo = e.Ubicacion.Nombre,
                    SectorEquipo = e.Sector.Nombre
                })
                .ToPagedList(page, 10);

            CalcularIndicadoresParaEquipos(fechaInicioDT, fechaFinDT, modelList);

            return PartialView("_IndicadoresPorUMDNSTable", modelList);
        }

        /// <summary>
        /// Agrega los valores de los indicadores para los elementos de la IPagedList pasada como parámetro.
        /// </summary>
        /// <param name="fechaInicioDT"></param>
        /// <param name="fechaFinDT"></param>
        /// <param name="modelList"></param>
        private void CalcularIndicadoresParaEquipos(DateTime fechaInicioDT, DateTime fechaFinDT, IPagedList<IndicadoresEquipoPorUbicacionViewModel> modelList)
        {
            foreach (var i in modelList)
            {
                i.TiempoIndisponibilidad = indicadoresSrv.TiempoIndisponibilidad(i.EquipoId, fechaInicioDT, fechaFinDT);
                i.TiempoMedioEntreFallas = indicadoresSrv.TiempoMedioEntreFallas(i.EquipoId, fechaInicioDT, fechaFinDT);
                i.TiempoMedioReparacion = indicadoresSrv.TiempoMedioParaReparar(i.EquipoId, fechaInicioDT, fechaFinDT);
            }
        }

        /// <summary>
        /// Acción AJAX que devuelve los indicadores para un solo equipo.
        /// </summary>
        /// <returns></returns>
        public ActionResult IndicadoresEquipo(int equipoId, string fechaInicio, string fechaFin)
        {
            var fechaInicioDT = DateTime.Parse(fechaInicio);
            var fechaFinDT = DateTime.Parse(fechaFin);

            var equipo = db.Equipos.Find(equipoId);

            var model = new IndicadoresEquipoViewModel
            {
                TiempoIndisponibilidad = indicadoresSrv.TiempoIndisponibilidad(equipoId, fechaInicioDT, fechaFinDT),
                TiempoMedioEntreFallas = indicadoresSrv.TiempoMedioEntreFallas(equipoId, fechaInicioDT, fechaFinDT),
                TiempoMedioReparacion = indicadoresSrv.TiempoMedioParaReparar(equipoId, fechaInicioDT, fechaFinDT)
            };

            return PartialView("_IndicadoresTable", model);
        }        

        /// <summary>
        /// Calcula los tiempos medios de reparación de los equipos para los sectores y ubicación,
        /// entre las fechas pasadas como parámetros.
        /// </summary>
        /// <param name="sectoresIds">String con los ids de los sectores a buscar separados por coma.</param>
        /// <param name="ubicacionId">Id de la ubicación seleccionada por el usuario.</param>
        /// <returns>Devuelve un diccionario en formato JSON con los tiempos medios de reparación de los equipos.</returns>
        public JsonResult ParetoTMPRDataUbicacion(string fechaInicio, string fechaFin, int? ubicacionId, int? sectorId)
        {
            var fechaInicioDT = DateTime.Parse(fechaInicio);
            var fechaFinDT = DateTime.Parse(fechaFin);
            var chartData = indicadoresSrv.ParetoChartDataTMPR(ubicacionId, sectorId, fechaInicioDT, fechaFinDT);

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Calcula los tiempos de medios entre fallas de los equipos para los sectores y ubicación,
        /// entre las fechas pasadas como parámetros.
        /// </summary>
        /// <param name="sectoresIds">String con los ids de los sectores a buscar separados por coma.</param>
        /// <param name="ubicacionId">Id de la ubicación seleccionada por el usuario.</param>
        /// <returns>Devuelve un diccionario en formato JSON con los tiempos medios entre fallas de los equipos.</returns>
        public JsonResult ParetoTMEFDataUbicacion(string fechaInicio, string fechaFin, int? ubicacionId, int? sectorId)
        {
            var fechaInicioDT = DateTime.Parse(fechaInicio);
            var fechaFinDT = DateTime.Parse(fechaFin);
            var chartData = indicadoresSrv.ParetoChartDataTMEF(ubicacionId, sectorId, fechaInicioDT, fechaFinDT);

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Calcula los tiempos de indisponibolidad de los equipos para los sectores y ubicación,
        /// entre las fechas pasadas como parámetros.
        /// </summary>
        /// <param name="sectoresIds">String con los ids de los sectores a buscar separados por coma.</param>
        /// <param name="ubicacionId">Id de la ubicación seleccionada por el usuario.</param>
        /// <returns>Devuelve un diccionario en formato JSON con los tiempos de indisponibilidad de los equipos.</returns>
        public JsonResult ParetoTIDataUbicacion(string fechaInicio, string fechaFin, int? ubicacionId, int? sectorId)
        {
            var fechaInicioDT = DateTime.Parse(fechaInicio);
            var fechaFinDT = DateTime.Parse(fechaFin);
            var chartData = indicadoresSrv.ParetoChartDataTI(ubicacionId, sectorId, fechaInicioDT, fechaFinDT);

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Calcula los tiempos medios de reparación de los equipos para los equipos con UMDNS y ubicación,
        /// entre las fechas pasadas como parámetros.
        /// </summary>
        /// <param name="sectoresIds">String con los ids de los sectores a buscar separados por coma.</param>
        /// <param name="ubicacionId">Id de la ubicación seleccionada por el usuario.</param>
        /// <returns>Devuelve un diccionario en formato JSON con los tiempos medios de reparación de los equipos.</returns>
        public JsonResult ParetoTMPRDataUMDNS(string fechaInicio, string fechaFin, string umdns, int? ubicacionId)
        {
            var fechaInicioDT = DateTime.Parse(fechaInicio);
            var fechaFinDT = DateTime.Parse(fechaFin);
            var chartData = indicadoresSrv.ParetoChartDataTMPR(umdns, ubicacionId, fechaInicioDT, fechaFinDT);

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Calcula los tiempos de medios entre fallas de los equipos para los equipos con UMDNS y ubicación,
        /// entre las fechas pasadas como parámetros.
        /// </summary>
        /// <param name="sectoresIds">String con los ids de los sectores a buscar separados por coma.</param>
        /// <param name="ubicacionId">Id de la ubicación seleccionada por el usuario.</param>
        /// <returns>Devuelve un diccionario en formato JSON con los tiempos medios entre fallas de los equipos.</returns>
        public JsonResult ParetoTMEFDataUMDNS(string fechaInicio, string fechaFin, string umdns, int? ubicacionId)
        {
            var fechaInicioDT = DateTime.Parse(fechaInicio);
            var fechaFinDT = DateTime.Parse(fechaFin);
            var chartData = indicadoresSrv.ParetoChartDataTMEF(umdns, ubicacionId, fechaInicioDT, fechaFinDT);

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Calcula los tiempos de indisponibolidad de los equipos con UMDNS y ubicación,
        /// entre las fechas pasadas como parámetros.
        /// </summary>
        /// <param name="sectoresIds">String con los ids de los sectores a buscar separados por coma.</param>
        /// <param name="ubicacionId">Id de la ubicación seleccionada por el usuario.</param>
        /// <returns>Devuelve un diccionario en formato JSON con los tiempos de indisponibilidad de los equipos.</returns>
        public JsonResult ParetoTIDataUMDNS(string fechaInicio, string fechaFin, string umdns, int? ubicacionId)
        {
            var fechaInicioDT = DateTime.Parse(fechaInicio);
            var fechaFinDT = DateTime.Parse(fechaFin);
            var chartData = indicadoresSrv.ParetoChartDataTI(umdns, ubicacionId, fechaInicioDT, fechaFinDT);

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
    }
}