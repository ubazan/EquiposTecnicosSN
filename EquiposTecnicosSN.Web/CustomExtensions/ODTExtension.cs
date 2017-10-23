using EquiposTecnicosSN.Entities.Mantenimiento;
using Salud.Security.SSO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EquiposTecnicosSN.Web.CustomExtensions
{
    public static class ODTExtension
    {
        private static string OdtMCorrectivoController = "ODTMantenimientoCorrectivo";
        private static string OdtMPreventivoController = "ODTMantenimientoPreventivo";
        private static string OdtBaseController = "OrdenesDeTrabajo";

        public static string WebController(this OrdenDeTrabajo odt)
        {

            if (odt is OrdenDeTrabajoMantenimientoCorrectivo)
            {
                return OdtMCorrectivoController;
            }

            if (odt is OrdenDeTrabajoMantenimientoPreventivo)
            {
                return OdtMPreventivoController;
            }

            return OdtBaseController;
        }
    }
}