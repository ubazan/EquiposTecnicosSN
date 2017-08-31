using EquiposTecnicosSN.Entities.Mantenimiento;
using EquiposTecnicosSN.Web.CustomExtensions;
using EquiposTecnicosSN.Web.DataContexts;
using Salud.Security.SSO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class SolicitudesRepuestoServicioController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();

        // GET: SolicitudesRepuestoServicio/DetailsSolicitud
        [HttpGet]
        public ActionResult DetailsSolicitud(int id)
        {
            var solicitud = db.SolicitudesRepuestosServicios.Find(id);
            return PartialView("_DetailSolicitudRepuestoServicioContent", solicitud);
        }

        // GET: */OrderReplacementService/id
        [HttpGet]
        public ActionResult OrderReplacementService(int id)
        {
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre");
            var odt = db.OrdenesDeTrabajo.Find(id);
            var model = new SolicitudRepuestoServicio
            {
                OrdenDeTrabajoId = id,
                OrdenDeTrabajo = odt,
                FechaInicio = DateTime.Now,
                Repuesto = new Repuesto(),
            };
            return View(model);
        }

        // POST: */OrderReplacementService
        [HttpPost]
        public async Task<ActionResult> OrderReplacementService(SolicitudRepuestoServicio solicitud)
        {
            SSOHelper.Authenticate();
            if (SSOHelper.CurrentIdentity == null)
            {
                string ssoUrl = SSOHelper.Configuration["SSO_URL"] as string;
                Response.Redirect(ssoUrl + "/Login.aspx");
            }

            var orden = db.OrdenesDeTrabajo.Find(solicitud.OrdenDeTrabajoId);
            solicitud.OrdenDeTrabajo = orden;

            if (solicitud.Repuesto.Codigo != null && solicitud.Repuesto.Nombre != null)
            {
                var repuesto = await db.Repuestos.Where(r => r.Codigo == solicitud.Repuesto.Codigo).SingleOrDefaultAsync();

                if (repuesto != null)
                {
                    solicitud.Repuesto = repuesto;
                }
            }
            else
            {
                solicitud.Repuesto = null;
            }

            //Es necesario que haya algun dato para guardar la solicitud
            if (solicitud.ProveedorId != null 
                || solicitud.Comentarios != null
                || solicitud.Repuesto != null)
            {
                solicitud.UsuarioInicio = (SSOHelper.CurrentIdentity != null ? SSOHelper.CurrentIdentity.Fullname : "Usuario Anónimo"); 
                db.SolicitudesRepuestosServicios.Add(solicitud);
                orden.Estado = OrdenDeTrabajoEstado.EsperaRepuesto;

                db.Entry(orden).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return RedirectToAction("Details", orden.WebController() ,new { id = solicitud.OrdenDeTrabajoId });
            }

            ModelState.AddModelError("", "Para guardar la solicitud debe registrarse el proveedor, las observaciones o el repuesto.");
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre", solicitud.ProveedorId);
            return View(solicitud);
        }


        // GET: */DetailsSolicitudRepuestoServicio/id
        [HttpGet]
        public ActionResult Details(int id)
        {
            var srs = db.SolicitudesRepuestosServicios.Find(id);
            return View(srs);
        }

        // POST: */Close
        [HttpPost]
        public async Task<JsonResult> Close(int solicitudId)
        {
            SSOHelper.Authenticate();
            if (SSOHelper.CurrentIdentity == null)
            {
                string ssoUrl = SSOHelper.Configuration["SSO_URL"] as string;
                Response.Redirect(ssoUrl + "/Login.aspx");
            }

            var sRespuestoServicio = db.SolicitudesRepuestosServicios.Find(solicitudId);

            //Descuento del stock la cantidad de repuestos
            if (sRespuestoServicio.RepuestoId != null)
            {
                var stockRepuesto = await db.StockRepuestos
                    .Where(sr => sr.RepuestoId == sRespuestoServicio.RepuestoId)
                    .SingleOrDefaultAsync();

                if (stockRepuesto != null) 
                {
                    if (stockRepuesto.CantidadDisponible <= sRespuestoServicio.CantidadRepuesto)
                    {
                        stockRepuesto.CantidadDisponible -= sRespuestoServicio.CantidadRepuesto;
                        db.Entry(stockRepuesto).State = EntityState.Modified;
                    }                   
                }
            }

            sRespuestoServicio.FechaCierre = DateTime.Now;
            sRespuestoServicio.UsuarioCierre = (SSOHelper.CurrentIdentity != null ? SSOHelper.CurrentIdentity.Fullname : "Usuario Anónimo");          
            db.Entry(sRespuestoServicio).State = EntityState.Modified;
            await db.SaveChangesAsync();

            //Valido si hay solicitudes abiertas para la orden de trabajo
            var solicitudesAbiertasCount = await db.SolicitudesRepuestosServicios
                .Where(s => s.OrdenDeTrabajoId == sRespuestoServicio.OrdenDeTrabajoId)
                .Where(s => s.FechaCierre == null)
                .CountAsync();

            if (solicitudesAbiertasCount == 0)
            {
                var orden = db.OrdenesDeTrabajo.Find(sRespuestoServicio.OrdenDeTrabajoId);
                orden.Estado = OrdenDeTrabajoEstado.Abierta;
                db.Entry(orden).State = EntityState.Modified;
            }

            await db.SaveChangesAsync();

            return Json(new
            {
                result = "success",
                solicitudId = solicitudId,
                updateEstado = solicitudesAbiertasCount == 0
            });
        }

    }
}