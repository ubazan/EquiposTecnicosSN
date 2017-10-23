using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Entities.Mantenimiento;
using EquiposTecnicosSN.Web.Models;
using EquiposTecnicosSN.Entities.Equipos;
using Salud.Security.SSO;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class ODTMantenimientoCorrectivoController : ODTController
    {

        // GET: OrdenesDeTrabajo/Details/5
        [HttpGet]
        override public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenDeTrabajoMantenimientoCorrectivo ordenDeTrabajo = db.ODTMantenimientosCorrectivos.Find(id);
            if (ordenDeTrabajo == null)
            {
                return HttpNotFound();
            }
            ordenDeTrabajo.SolicitudesRespuestos = db.SolicitudesRepuestosServicios.Where(s => s.OrdenDeTrabajoId == id).ToList();

            return View(ordenDeTrabajo);
        }

        // GET: OrdenesDeTrabajo/CreateForEquipo/id
        [HttpGet]
        override public ActionResult CreateForEquipo(int id)
        {
            //SSOHelper.Authenticate();
            //if (SSOHelper.CurrentIdentity == null)
            //{
            //    string ssoUrl = SSOHelper.Configuration["SSO_URL"] as string;
            //    Response.Redirect(ssoUrl + "/Login.aspx");
            //}

            var equipo = db.Equipos.Find(id);
            var vm = new MCViewModel();
            vm.Odt = new OrdenDeTrabajoMantenimientoCorrectivo
            {
                EquipoId = equipo.EquipoId,
                Equipo = equipo,
                Estado = OrdenDeTrabajoEstado.Abierta,
                FechaInicio = DateTime.Now,
                NumeroReferencia = DateTime.Now.ToString("yyyyMMddHHmmssff"),
                Prioridad = OrdenDeTrabajoPrioridad.Normal,
                UsuarioInicio = (SSOHelper.CurrentIdentity != null ? SSOHelper.CurrentIdentity.Fullname : "Usuario Anónimo")
            };

            vm.NuevaObservacion = NuevaObservacion();

            return View(vm);
        }

        // POST: OrdenesDeTrabajo/CreateForEquipo
        [HttpPost]
        public async Task<ActionResult> CreateForEquipo(MCViewModel vm)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                vm.Odt.FechaInicio = DateTime.Now;

                //estado del equipo
                var equipo = db.Equipos.Find(vm.Odt.EquipoId);
                equipo.Estado = (vm.Odt.EquipoParado ? EstadoDeEquipo.NoFuncionalRequiereReparacion : EstadoDeEquipo.FuncionalRequiereReparacion);
                db.Entry(equipo).State = EntityState.Modified;

                db.ODTMantenimientosCorrectivos.Add(vm.Odt);
                //Observacion
                SaveNuevaObservacion(vm.NuevaObservacion, vm.Odt);

                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = vm.Odt.OrdenDeTrabajoId });
            }

            return View(vm);
        }

        // GET: OrdenesDeTrabajo/FillDiagnose/id
        [HttpGet]
        public ActionResult FillDiagnose(int id)
        {
            var odt = db.ODTMantenimientosCorrectivos.Find(id);
            //Se agrego para que en la vista se pueda ver la lista de proveedores -- Jose Gutierrez 29/05/2017
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre", odt.ProveedorId);
            ViewBag.GastoProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre", odt.Gastos.Select(p => p.GastoProveedorId));
            var model = new MCViewModel();
            model.Odt = odt;
            model.NuevaObservacion = NuevaObservacion();
            return View(model);
        }

        // POST: OrdenesDeTrabajo/FillDiagnose
        [HttpPost]
        public async Task<ActionResult> FillDiagnose(MCViewModel vm, IEnumerable<GastoOrdenDeTrabajo> gastos)
        {
            //SSOHelper.Authenticate();
            //if (SSOHelper.CurrentIdentity == null)
            //{
            //    string ssoUrl = SSOHelper.Configuration["SSO_URL"] as string;
            //    Response.Redirect(ssoUrl + "/Login.aspx");
            //}

            if (vm.Odt.Diagnostico == null &&
                vm.Odt.Gastos == null &&
                gastos == null)
            {
                return View(vm);
            }

            var orden = db.ODTMantenimientosCorrectivos.Find(vm.Odt.OrdenDeTrabajoId);
            //datos de diagnostico
            orden.Diagnostico = vm.Odt.Diagnostico;
            orden.ReparoTercero = vm.Odt.ReparoTercero;
            orden.FechaInicioTercero = vm.Odt.FechaInicioTercero;
            orden.ProveedorId = vm.ProveedorId;
            orden.FechaDiagnostico = DateTime.Now;
            orden.UsuarioDiagnostico = (SSOHelper.CurrentIdentity != null ? SSOHelper.CurrentIdentity.Fullname : "Usuario Anónimo");
            //gastos
            if (gastos != null)
            {
                SaveGastos(gastos, orden.OrdenDeTrabajoId);
            }
            //nueva observacion
            SaveNuevaObservacion(vm.NuevaObservacion, orden);

            db.Entry(orden).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = orden.OrdenDeTrabajoId });
        }


        // GET: OrdenesDeTrabajo/Reparar/id
        [HttpGet]
        override public ActionResult Reparar(int id)
        {
            var odt = db.ODTMantenimientosCorrectivos.Find(id);
            var model = new MCViewModel();
            model.Odt = odt;
            model.NuevaObservacion = NuevaObservacion();
            return View(model);
        }

        // POST: OrdenesDeTrabajo/FillRepair
        [HttpPost]
        public async Task<ActionResult> Reparar(MCViewModel vm, IEnumerable<GastoOrdenDeTrabajo> gastos)
        {

            //SSOHelper.Authenticate();
            //if (SSOHelper.CurrentIdentity == null)
            //{
            //    string ssoUrl = SSOHelper.Configuration["SSO_URL"] as string;
            //    Response.Redirect(ssoUrl + "/Login.aspx");
            //}

            OrdenDeTrabajoMantenimientoCorrectivo orden = await db.ODTMantenimientosCorrectivos
                .Include(o => o.SolicitudesRespuestos)
                .Where(o => o.OrdenDeTrabajoId == vm.Odt.OrdenDeTrabajoId)
                .SingleOrDefaultAsync();

            if (orden == null)
            {
                return HttpNotFound();
            }

            if (vm.Odt.DetalleReparacion != null)
            {
                orden.DetalleReparacion = vm.Odt.DetalleReparacion;
                orden.CausaRaiz = vm.Odt.CausaRaiz;
                orden.Limpieza = vm.Odt.Limpieza;
                orden.VerificacionFuncionamiento = vm.Odt.VerificacionFuncionamiento;

                orden.Estado = OrdenDeTrabajoEstado.Reparada;
                orden.FechaReparacion = DateTime.Now;
                //orden.FechaCierre = DateTime.Now;
                orden.UsuarioReparacion = (SSOHelper.CurrentIdentity != null ? SSOHelper.CurrentIdentity.Fullname : "Usuario Anónimo");
                //orden.UsuarioCierre = (SSOHelper.CurrentIdentity != null ? SSOHelper.CurrentIdentity.Fullname : "Usuario Anónimo");

                //estado del equipo
                var equipo = db.Equipos.Find(orden.EquipoId);
                equipo.Estado = (vm.Odt.VerificacionFuncionamiento ? EstadoDeEquipo.Funcional : EstadoDeEquipo.NoFuncionalRequiereReparacion);
                db.Entry(equipo).State = EntityState.Modified;

                //gastos
                SaveGastos(gastos, orden.OrdenDeTrabajoId);

                //observaciones
                SaveNuevaObservacion(vm.NuevaObservacion, orden);

                //solicitudes
                CloseSolicitudesRepuestos(orden);

                db.Entry(orden).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = vm.Odt.OrdenDeTrabajoId });
            }

            return View(vm);
        }

        // GET: OrdenesDeTrabajo/Close/id
        [HttpGet]
        override public ActionResult Close(int id)
        {
            var odt = db.ODTMantenimientosCorrectivos.Find(id);           
            var model = new MCViewModel();
            model.Odt = odt;
            model.NuevaObservacion = NuevaObservacion();          
            return View(model);
        }

        // POST: OrdenesDeTrabajo/FillRepair
        [HttpPost]
        public async Task<ActionResult> Close(MCViewModel vm, IEnumerable<GastoOrdenDeTrabajo> gastos)
        {

            //SSOHelper.Authenticate();
            //if (SSOHelper.CurrentIdentity == null)
            //{
            //    string ssoUrl = SSOHelper.Configuration["SSO_URL"] as string;
            //    Response.Redirect(ssoUrl + "/Login.aspx");
            //}

            OrdenDeTrabajoMantenimientoCorrectivo orden = await db.ODTMantenimientosCorrectivos
                .Include(o => o.SolicitudesRespuestos)
                .Where(o => o.OrdenDeTrabajoId == vm.Odt.OrdenDeTrabajoId)
                .SingleOrDefaultAsync();

            if (orden == null)
            {
                return HttpNotFound();
            }

            if (orden.DetalleReparacion != null)
            {


                orden.Estado = OrdenDeTrabajoEstado.Cerrada;
                //orden.FechaReparacion = DateTime.Now;
                orden.FechaCierre = DateTime.Now;
                //orden.UsuarioReparacion = (SSOHelper.CurrentIdentity != null ? SSOHelper.CurrentIdentity.Fullname : "Usuario Anónimo");
                orden.UsuarioCierre = (SSOHelper.CurrentIdentity != null ? SSOHelper.CurrentIdentity.Fullname : "Usuario Anónimo");

                //gastos
                SaveGastos(gastos, orden.OrdenDeTrabajoId);

                //observaciones
                SaveNuevaObservacion(vm.NuevaObservacion, orden);

                //solicitudes
                CloseSolicitudesRepuestos(orden);

                db.Entry(orden).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = vm.Odt.OrdenDeTrabajoId });
            }

            return View(vm);
        }

        // GET: OrdenesDeTrabajo/EditGastos/id
        [HttpGet]
        override public ActionResult EditGastos(int id)
        {
            var model = db.ODTMantenimientosCorrectivos.Find(id);
            ViewBag.GastoProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre", model.Gastos.Select(p => p.GastoProveedorId));
            return View(model);
        }

        // POST: OrdenesDeTrabajo/EditGastos
        [HttpPost]
        public async Task<ActionResult> EditGastos(int ordenDeTrabajoId, IEnumerable<GastoOrdenDeTrabajo> gastos)
        {
            var orden = db.ODTMantenimientosCorrectivos.Find(ordenDeTrabajoId);
            //gastos
            SaveGastos(gastos, orden.OrdenDeTrabajoId);

            db.Entry(orden).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = orden.OrdenDeTrabajoId });
        }

        // GET: OrdenesDeTrabajo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenDeTrabajoMantenimientoCorrectivo ordenDeTrabajo = await db.ODTMantenimientosCorrectivos.FindAsync(id);
            if (ordenDeTrabajo == null)
            {
                return HttpNotFound();
            }
            ViewBag.EquipoId = new SelectList(db.Equipos, "EquipoId", "NombreCompleto", ordenDeTrabajo.EquipoId);
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre", ordenDeTrabajo.ProveedorId);
            return View(ordenDeTrabajo);
        }

        // POST: OrdenesDeTrabajo/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(OrdenDeTrabajoMantenimientoCorrectivo ordenDeTrabajo)
        {
            if (ModelState.IsValid)
            {
                //gastos
                SaveGastos(ordenDeTrabajo.Gastos, ordenDeTrabajo.OrdenDeTrabajoId);

                db.Entry(ordenDeTrabajo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("IndexMantenimientos");
            }
            ViewBag.EquipoId = new SelectList(db.Equipos, "EquipoId", "NombreCompleto", ordenDeTrabajo.EquipoId);
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre", ordenDeTrabajo.ProveedorId);
            return View(ordenDeTrabajo);
        }

        // GET: OrdenesDeTrabajo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenDeTrabajoMantenimientoCorrectivo ordenDeTrabajo = await db.ODTMantenimientosCorrectivos.FindAsync(id);
            if (ordenDeTrabajo == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeTrabajo);
        }

        // POST: OrdenesDeTrabajo/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OrdenDeTrabajoMantenimientoCorrectivo ordenDeTrabajo = await db.ODTMantenimientosCorrectivos.FindAsync(id);
            db.ODTMantenimientosCorrectivos.Remove(ordenDeTrabajo);
            await db.SaveChangesAsync();
            return RedirectToAction("IndexMantenimientos");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Autocompletes the proveedor.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public ActionResult AutocompleteProveedor(string term)
        {
            var model = db.Proveedores.Where(p => p.Nombre.Contains(term))
                .Take(6)
                .Select(p => new
                {
                    label = p.Nombre,
                    value = p.ProveedorId
                });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //GET: Impresion OT
        public virtual ActionResult ImpresionOT(int? equipoId)
        {
            var model = db.Equipos.Find(equipoId);
            return PartialView("_ImpresionOTContent", model);
        }
    }
}
