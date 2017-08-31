using EquiposTecnicosSN.Entities.Equipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EquiposTecnicosSN.Web.CustomExtensions
{
    public static class EquiposExtension
    {
        private static string EquipamientosEdiliciosController = "EquipamientosEdilicios";
        private static string EquiposCirugiaController = "EquiposCirugia";
        private static string EquiposClimatizacionController = "EquiposClimatizacion";
        private static string EquiposEndoscopiaController = "EquiposEndoscopia";
        private static string EquiposGasesMedicinalesController = "EquiposGasesMedicinales";
        private static string EquiposImagenesController = "EquiposImagenes";
        private static string EquiposInformaticaController = "EquiposInformatica";
        private static string EquiposLucesController = "EquiposLuces";
        private static string EquiposMonitoreoController = "EquiposMonitoreo";
        private static string EquiposOdontologiaController = "EquiposOdontologia";
        private static string EquiposOtrosController = "EquiposOtros";
        private static string EquiposPruebasDeDiagnosticoController = "EquiposPruebasDeDiagnostico";
        private static string EquiposRehabilitacionController = "EquiposRehabilitacion";
        private static string EquiposSoporteDeVidaController = "EquiposSoporteDeVida";
        private static string EquiposTerapeuticaController = "EquiposTerapeutica";
        private static string EquiposBaseController = "EquiposBase";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipo"></param>
        /// <returns></returns>
        public static string WebController(this Equipo equipo)
        {

            if (equipo is EquipamientoEdilicio)
            {
                return EquipamientosEdiliciosController;
            }

            if (equipo is EquipoCirugia)
            {
                return EquiposCirugiaController;
            }

            if (equipo is EquipoClimatizacion)
            {
                return EquiposClimatizacionController;
            }

            if (equipo is EquipoEndoscopia)
            {
                return EquiposEndoscopiaController;
            }

            if (equipo is EquipoGasesMedicinales)
            {
                return EquiposGasesMedicinalesController;
            }

            if (equipo is EquipoImagen)
            {
                return EquiposImagenesController;
            }
            
            if (equipo is EquipoInformatica)
            {
                return EquiposInformaticaController;
            }

            if (equipo is EquipoLuces)
            {
                return EquiposLucesController;
            }

            if (equipo is EquipoMonitoreo)
            {
                return EquiposMonitoreoController;
            }

            if (equipo is EquipoOdontologia)
            {
                return EquiposOdontologiaController;
            }

            if (equipo is EquipoOtro)
            {
                return EquiposOtrosController;
            }

            if (equipo is EquipoPruebaDeDiagnostico)
            {
                return EquiposPruebasDeDiagnosticoController;
            }

            if (equipo is EquipoRehabilitacion)
            {
                return EquiposRehabilitacionController;
            }

            if (equipo is EquipoSoporteDeVida)
            {
                return EquiposSoporteDeVidaController;
            }

            if (equipo is EquipoTerapeutica)
            {
                return EquiposTerapeuticaController;
            }

            return EquiposBaseController;
        }
    }



}