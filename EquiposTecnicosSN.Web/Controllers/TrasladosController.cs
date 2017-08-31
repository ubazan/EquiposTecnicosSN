using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EquiposTecnicosSN.Entities.Equipos.Info;
using EquiposTecnicosSN.Web.DataContexts;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class TrasladosController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipoId"></param>
        /// <returns></returns>
        public ActionResult LoadTrasladarEquipo(int equipoId)
        {
            var equipo = db.Equipos.Find(equipoId);
            var model = new Traslado
            {
                EquipoId = equipo.EquipoId,
                UbicacionOrigenId = equipo.UbicacionId
            };
            ViewBag.UbicacionOrigenId = new SelectList(db.Ubicaciones, "UbicacionId", "Nombre", equipo.UbicacionId);
            ViewBag.UbicacionDestinoId = new SelectList(db.Ubicaciones, "UbicacionId", "Nombre");
            return PartialView("_TrasladarEquipoContent", model);
        }

        [HttpPost]
        public JsonResult TrasladarEquipo(int EquipoId, int UbicacionOrigenId, int UbicacionDestinoId)
        {
            var equipo = db.Equipos.Find(EquipoId);
            equipo.UbicacionId = UbicacionDestinoId;
            db.Entry(equipo).State = EntityState.Modified;

            var traslado = new Traslado
            {
                EquipoId = equipo.EquipoId,
                UbicacionOrigenId = UbicacionOrigenId,
                UbicacionDestinoId = UbicacionDestinoId,
                FechaTraslado = DateTime.Now
            };

            db.Traslados.Add(traslado);
            db.SaveChanges();

            var result = new {
                RowId = EquipoId,
                Value = db.Ubicaciones.Find(UbicacionDestinoId).Nombre
            };

            return Json(result);
        }



        // GET: Traslados
        public async Task<ActionResult> Index()
        {
            var traslados = db.Traslados.Include(t => t.Equipo).Include(t => t.UbicacionOrigen).Include(t => t.UbicacionDestino);
            return View(await traslados.ToListAsync());
        }

        // GET
        public async Task<JsonResult> GetUbicacionEquipo(int equipoId)
        {
            var equipo = await db.Equipos.Include(e => e.Ubicacion).Where(e => e.EquipoId == equipoId).FirstAsync();
            return Json(equipo.UbicacionId);
        }


        // GET: Traslados/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Traslado traslado = await db.Traslados.FindAsync(id);
            if (traslado == null)
            {
                return HttpNotFound();
            }
            return View(traslado);
        }

        // GET: Traslados/Create
        public ActionResult Create()
        {
            ViewBag.EquipoId = new SelectList(db.Equipos, "EquipoId", "NombreCompleto");
            ViewBag.UbicacionOrigenId = new SelectList(db.Ubicaciones, "UbicacionId", "Nombre");
            ViewBag.UbicacionDestinoId = new SelectList(db.Ubicaciones, "UbicacionId", "Nombre");
            return View();
        }

        // POST: Traslados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "TrasladoId,EquipoId,FechaTraslado,UbicacionOrigenId,UbicacionDestinoId")] Traslado traslado)
        {
            if (ModelState.IsValid)
            {
                db.Traslados.Add(traslado);

                var equipoTrasladado = await db.Equipos.FindAsync(traslado.EquipoId);
                equipoTrasladado.UbicacionId = traslado.UbicacionDestinoId;
                db.Entry(equipoTrasladado).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EquipoId = new SelectList(db.Equipos, "EquipoId", "NombreCompleto", traslado.EquipoId);
            ViewBag.UbicacionOrigenId = new SelectList(db.Ubicaciones, "UbicacionId", "Nombre", traslado.UbicacionOrigenId);
            ViewBag.UbicacionDestinoId = new SelectList(db.Ubicaciones, "UbicacionId", "Nombre", traslado.UbicacionDestinoId);
            return View(traslado);
        }

        // GET: Traslados/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Traslado traslado = await db.Traslados.FindAsync(id);
            if (traslado == null)
            {
                return HttpNotFound();
            }
            ViewBag.EquipoId = new SelectList(db.Equipos, "EquipoId", "NombreCompleto", traslado.EquipoId);
            ViewBag.UbicacionOrigenId = new SelectList(db.Ubicaciones, "UbicacionId", "Nombre", traslado.UbicacionOrigenId);
            ViewBag.UbicacionDestinoId = new SelectList(db.Ubicaciones, "UbicacionId", "Nombre", traslado.UbicacionDestinoId);
            return View(traslado);
        }

        // POST: Traslados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "TrasladoId,EquipoId,FechaTraslado,UbicacionOrigenId,UbicacionDestinoId")] Traslado traslado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(traslado).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EquipoId = new SelectList(db.Equipos, "EquipoId", "NombreCompleto", traslado.EquipoId);
            ViewBag.UbicacionOrigenId = new SelectList(db.Ubicaciones, "UbicacionId", "Nombre", traslado.UbicacionOrigenId);
            ViewBag.UbicacionDestinoId = new SelectList(db.Ubicaciones, "UbicacionId", "Nombre", traslado.UbicacionDestinoId);
            return View(traslado);
        }

        // GET: Traslados/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Traslado traslado = await db.Traslados.FindAsync(id);
            if (traslado == null)
            {
                return HttpNotFound();
            }
            return View(traslado);
        }

        // POST: Traslados/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Traslado traslado = await db.Traslados.FindAsync(id);
            db.Traslados.Remove(traslado);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
