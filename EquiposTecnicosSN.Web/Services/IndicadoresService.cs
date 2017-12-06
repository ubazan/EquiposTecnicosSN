using System;
using System.Collections.Generic;
using System.Linq;

namespace EquiposTecnicosSN.Web.Services
{
    /// <summary>
    /// Servicio con metodos para calcular los indicadores Tiempo de indisponibilidad,
    /// Tiempo medio de reparación y Tiempo medio entre fallas.
    /// </summary>
    public class IndicadoresService : BaseService
    {

        /// <summary>
        /// Devuelve el tiempo de indisponibilidad de un equipo entre las fechas indicadas.
        /// </summary>
        /// <param name="equipoId">ID del equipo.</param>
        /// <param name="fechaInicio">Fecha de inicio del período.</param>
        /// <param name="fechaFin">Fecha de fin del período.</param>
        /// <returns>Porcentaje con dos decimales.</returns>
        public double TiempoIndisponibilidad(int equipoId, DateTime fechaInicio, DateTime fechaFin)
        {

            var odtsMC = db.ODTMantenimientosCorrectivos
                .Where(odt => odt.EquipoId == equipoId)
                .Where(odt => odt.EquipoParado)
                .Where(odt => odt.FechaCierre != null)
                .Where(odt => DateTime.Compare(odt.FechaInicio, fechaInicio) >= 0)
                .Where(odt => DateTime.Compare(odt.FechaCierre.Value, fechaFin) <= 0)
                .ToList();

            var odtsMP = db.ODTMantenimientosPreventivos
                .Where(odt => odt.EquipoId == equipoId)
                .Where(odt => odt.FechaCierre != null)
                .Where(odt => DateTime.Compare(odt.FechaInicio, fechaInicio) >= 0)
                .Where(odt => DateTime.Compare(odt.FechaCierre.Value, fechaFin) <= 0)
                .ToList();

            var totalCount = odtsMC.Count + odtsMP.Count;

            if (totalCount == 0)
            {
                return 0;
            }

            var fechaCarga = db.Equipos.Find(equipoId).InformacionComercial.FechaCompra != null ? db.Equipos.Find(equipoId).InformacionComercial.FechaCompra : DateTime.MinValue;
            var tFuncionamientoEsperado = (fechaFin - fechaInicio).TotalHours;

            double sumaTiemposOdts = 0;
            foreach (var odt in odtsMC)
            {
                sumaTiemposOdts += (odt.FechaCierre.Value - odt.FechaInicio).TotalHours;
            }

            foreach (var odt in odtsMP)
            {
                sumaTiemposOdts += (odt.FechaCierre.Value - odt.FechaInicio).TotalHours;
            }

            return Math.Round((sumaTiemposOdts / tFuncionamientoEsperado) * 100, 4);
        }

        /// <summary>
        /// Devuelve el tiempo medio entre fallas de un equipo entre las fechas indicadas.
        /// </summary>
        /// <param name="equipoId">ID del equipo.</param>
        /// <param name="fechaInicio">Fecha de inicio del período.</param>
        /// <param name="fechaFin">Fecha de fin del período.</param>
        /// <returns>Tiempo en días con dos decimales.</returns>
        public double TiempoMedioEntreFallas(int equipoId, DateTime fechaInicio, DateTime fechaFin)
        {
            var odts = db.ODTMantenimientosCorrectivos
                .Where(odt => odt.EquipoId == equipoId)
                .Where(odt => odt.FechaCierre != null)
                .Where(odt => DateTime.Compare(odt.FechaInicio, fechaInicio) >= 0)
                .Where(odt => DateTime.Compare(odt.FechaCierre.Value, fechaFin) <= 0)
                .ToList();

            if (odts.Count == 0)
            {
                return 0;
            }

            //Comento ya que la forma de calcular el TMEF tiene objeciones. Ulises: 30/11/2017
            //double sumaTiemposFuncionamiento = 0;
            //for (int i = 0; i < odts.Count - 1; i++)
            //{
            //    var current = odts[i];
            //    if (i == 0)
            //    {
            //        sumaTiemposFuncionamiento += (current.FechaInicio - fechaInicio).TotalDays;
            //    }
            //    else if (i == odts.Count -1)
            //    {
            //        sumaTiemposFuncionamiento += (fechaFin - current.FechaInicio).TotalDays;
            //    }
            //    else
            //    {
            //        var prev = odts[i - 1];
            //        sumaTiemposFuncionamiento += (current.FechaInicio - prev.FechaCierre.Value).TotalDays;
            //    }
            //}
            double sumaTiempoTotal= 0;
            sumaTiempoTotal = (fechaFin - fechaInicio).TotalDays;

            return Math.Round(sumaTiempoTotal / odts.Count, 4);
        }

        /// <summary>
        /// Devuelve el tiempo medio de reparación de un equipo entre las fechas indicadas.
        /// </summary>
        /// <param name="equipoId">ID del equipo.</param>
        /// <param name="fechaInicio">Fecha de inicio del período.</param>
        /// <param name="fechaFin">Fecha de fin del período.</param>
        /// <returns>Tiempo en minutos con dos decimales.</returns>
        public double TiempoMedioParaReparar(int equipoId, DateTime fechaInicio, DateTime fechaFin)
        {
            double controlDias = 0;
            int controlODT = 0;
            var odts = db.ODTMantenimientosCorrectivos
                .Where(odt => odt.EquipoId == equipoId)
                .Where(odt => odt.FechaCierre != null)
                .Where(odt => DateTime.Compare(odt.FechaInicio, fechaInicio) > 0)
                .Where(odt => DateTime.Compare(odt.FechaCierre.Value, fechaFin) < 0)
                .Where(odt => odt.FechaReparacion != null)
                .ToList();

            if (odts.Count == 0)
            {
                return 0;
            }

            double sumaTiemposOdts = 0;
            foreach(var odt in odts)
            {
                sumaTiemposOdts += (odt.FechaReparacion.Value - odt.FechaInicio).TotalDays;
                if((odt.FechaReparacion.Value - odt.FechaInicio).TotalDays < 0)
                {
                    controlDias = (odt.FechaReparacion.Value - odt.FechaInicio).TotalDays;
                    controlODT = odt.OrdenDeTrabajoId;
                }
            }

            return Math.Round(sumaTiemposOdts / odts.Count, 4);
        }

        /// <summary>
        /// Devuelve un diccionario con los tiempos de indisponibilidad de los equipos que resulten de la búsqueda.
        /// </summary>
        /// <param name="ubicacionId">ID de la ubicación.</param>
        /// <param name="sectorId">ID del sector</param>
        /// <param name="fechaInicioDT">Fecha de inicio del período.</param>
        /// <param name="fechaFinDT">Fecha de fin del período.</param>
        /// <returns>Un diccionario.</returns>
        public Dictionary<string, double> ParetoChartDataTI(int? ubicacionId, int? sectorId, DateTime fechaInicioDT, DateTime fechaFinDT)
        {

            Dictionary<string, double> chartData = new Dictionary<string, double>();

            var equipos = db.Equipos
                .Where(e => sectorId == null || e.SectorId == sectorId)
                .Where(e => ubicacionId == null || e.UbicacionId == ubicacionId)
                .ToList();

            foreach (var equipo in equipos)
            {
                var label = equipo.NombreCompleto 
                    + (ubicacionId == null ? " - " + equipo.Ubicacion.Nombre : "") 
                    + (sectorId == null ? " - " + equipo.Sector.Nombre : "");

                chartData.Add(label, TiempoIndisponibilidad(equipo.EquipoId, fechaInicioDT, fechaFinDT));
            }

            return chartData.OrderByDescending(x => x.Value).ToDictionary(r => r.Key, r => r.Value);
        }

        /// <summary>
        /// Devuelve un diccionario con los tiempos medios de reparación de los equipos que resulten de la búsqueda.
        /// </summary>
        /// <param name="ubicacionId">ID de la ubicación.</param>
        /// <param name="sectorId">ID del sector</param>
        /// <param name="fechaInicioDT">Fecha de inicio del período.</param>
        /// <param name="fechaFinDT">Fecha de fin del período.</param>
        /// <returns>Un diccionario.</returns>
        public Dictionary<string, double> ParetoChartDataTMPR(int? ubicacionId, int? sectorId, DateTime fechaInicioDT, DateTime fechaFinDT)
        {

            Dictionary<string, double> chartData = new Dictionary<string, double>();

            var equipos = db.Equipos
                .Where(e => sectorId == null || e.SectorId == sectorId)
                .Where(e => ubicacionId == null || e.UbicacionId == ubicacionId)
                .ToList();

            foreach (var equipo in equipos)
            {
                var label = equipo.NombreCompleto
                    + (ubicacionId == null ? " - " + equipo.Ubicacion.Nombre : "")
                    + (sectorId == null ? " - " + equipo.Sector.Nombre : "");

                chartData.Add(label, TiempoMedioParaReparar(equipo.EquipoId, fechaInicioDT, fechaFinDT));
            }

            return chartData.OrderByDescending(x => x.Value).ToDictionary(r => r.Key, r => r.Value);
        }

        /// <summary>
        /// Devuelve un diccionario con los tiempos medios entre fallas de los equipos que resulten de la búsqueda.
        /// </summary>
        /// <param name="ubicacionId">ID de la ubicación.</param>
        /// <param name="sectorId">ID del sector</param>
        /// <param name="fechaInicioDT">Fecha de inicio del período.</param>
        /// <param name="fechaFinDT">Fecha de fin del período.</param>
        /// <returns>Un diccionario.</returns>
        public Dictionary<string, double> ParetoChartDataTMEF(int? ubicacionId, int? sectorId, DateTime fechaInicioDT, DateTime fechaFinDT)
        {

            Dictionary<string, double> chartData = new Dictionary<string, double>();

            var equipos = db.Equipos
                .Where(e => sectorId == null || e.SectorId == sectorId)
                .Where(e => ubicacionId == null || e.UbicacionId == ubicacionId)
                .ToList();

            foreach (var equipo in equipos)
            {
                var label = equipo.NombreCompleto
                    + (ubicacionId == null ? " - " + equipo.Ubicacion.Nombre : "")
                    + (sectorId == null ? " - " + equipo.Sector.Nombre : "");

                chartData.Add(label, TiempoMedioEntreFallas(equipo.EquipoId, fechaInicioDT, fechaFinDT));
            }

            return chartData.OrderByDescending(x => x.Value).ToDictionary(r => r.Key, r => r.Value);
        }

        /// <summary>
        /// Devuelve un diccionario con los tiempos de indisponibilidad de los equipos que resulten de la búsqueda.
        /// </summary>
        /// <param name="umdns">Código UMNDS.</param>
        /// <param name="ubicacionId">ID de la ubicación.</param>
        /// <param name="fechaInicioDT">Fecha de inicio del período.</param>
        /// <param name="fechaFinDT">Fecha de fin del período.</param>
        /// <returns>Un diccionario.</returns>
        public Dictionary<string, double> ParetoChartDataTI(string umdns, int? ubicacionId, DateTime fechaInicioDT, DateTime fechaFinDT)
        {

            Dictionary<string, double> chartData = new Dictionary<string, double>();

            var equipos = db.Equipos
                .Where(e => e.UMDNS == umdns)
                .Where(e => ubicacionId == null || e.UbicacionId == ubicacionId)
                .ToList();

            foreach (var equipo in equipos)
            {
                var label = equipo.NombreCompleto
                    + (ubicacionId == null ? " - " + equipo.Ubicacion.Nombre : "");

                chartData.Add(label, TiempoIndisponibilidad(equipo.EquipoId, fechaInicioDT, fechaFinDT));
            }

            return chartData.OrderByDescending(x => x.Value).ToDictionary(r => r.Key, r => r.Value);
        }


        /// <summary>
        /// Devuelve un diccionario con los tiempos medios de reparación de los equipos que resulten de la búsqueda.
        /// </summary>
        /// <param name="umdns">Código UMNDS.</param>
        /// <param name="ubicacionId">ID de la ubicación.</param>
        /// <param name="fechaInicioDT">Fecha de inicio del período.</param>
        /// <param name="fechaFinDT">Fecha de fin del período.</param>
        /// <returns>Un diccionario.</returns>
        public Dictionary<string, double> ParetoChartDataTMPR(string umdns, int? ubicacionId, DateTime fechaInicioDT, DateTime fechaFinDT)
        {

            Dictionary<string, double> chartData = new Dictionary<string, double>();

            var equipos = db.Equipos
                .Where(e => e.UMDNS == umdns)
                .Where(e => ubicacionId == null || e.UbicacionId == ubicacionId)
                .ToList();

            foreach (var equipo in equipos)
            {
                var label = equipo.NombreCompleto
                    + (ubicacionId == null ? " - " + equipo.Ubicacion.Nombre : "");

                chartData.Add(label, TiempoMedioParaReparar(equipo.EquipoId, fechaInicioDT, fechaFinDT));
            }

            return chartData.OrderByDescending(x => x.Value).ToDictionary(r => r.Key, r => r.Value);
        }

        /// <summary>
        /// Devuelve un diccionario con los tiempos medios entre fallas de los equipos que resulten de la búsqueda.
        /// </summary>
        /// <param name="umdns">Código UMNDS.</param>
        /// <param name="ubicacionId">ID de la ubicación.</param>
        /// <param name="fechaInicioDT">Fecha de inicio del período.</param>
        /// <param name="fechaFinDT">Fecha de fin del período.</param>
        /// <returns>Un diccionario.</returns>
        public Dictionary<string, double> ParetoChartDataTMEF(string umdns, int? ubicacionId, DateTime fechaInicioDT, DateTime fechaFinDT)
        {

            Dictionary<string, double> chartData = new Dictionary<string, double>();

            var equipos = db.Equipos
                .Where(e => e.UMDNS == umdns)
                .Where(e => ubicacionId == null || e.UbicacionId == ubicacionId)
                .ToList();

            foreach (var equipo in equipos)
            {
                var label = equipo.NombreCompleto
                    + (ubicacionId == null ? " - " + equipo.Ubicacion.Nombre : "");

                chartData.Add(label, TiempoMedioEntreFallas(equipo.EquipoId, fechaInicioDT, fechaFinDT));
            }

            return chartData.OrderByDescending(x => x.Value).ToDictionary(r => r.Key, r => r.Value);
        }
    }
}