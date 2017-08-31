using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Entities.Equipos;
using EquiposTecnicosSN.Entities.Mantenimiento;
using EquiposTecnicosSN.Web.DataContexts;


namespace EquiposTecnicosSN.Web.Services
{
    public class EquiposService : BaseService
    {
        public List<Equipo> EquiposFuncionales ()
        {
            return db.Equipos
                .Where(e => e.Estado == EstadoDeEquipo.Funcional || e.Estado == EstadoDeEquipo.FuncionalRequiereReparacion)
                .OrderBy(e => e.NombreCompleto)
                .ToList();
        }

        public int EquiposFuncionalesCount()
        {
            return db.Equipos
                .Where(e => e.Estado == EstadoDeEquipo.Funcional || e.Estado == EstadoDeEquipo.FuncionalRequiereReparacion)
                .OrderBy(e => e.NombreCompleto)
                .Count();
        }
    }
}