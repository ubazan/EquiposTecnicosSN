using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Web.DataContexts;
using EquiposTecnicosSN.Entities.Equipos.Info;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class InformacionHardwareController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();

        // JSON POST: InformacionHardware/GetMarcas/5
        public async Task<JsonResult> GetMarcas(int fabricanteId)
        {
            var list = await db.Marcas.Where(m => m.FabricanteId == fabricanteId).Select(m => new { m.MarcaId, m.Nombre}).ToListAsync();
            return Json(list);
        }

        // JSON POST: InformacionHardware/GetModelos/5
        public async Task<JsonResult> GetModelos(int marcaId)
        {
            var list = await db.Modelos.Where(m => m.MarcaId == marcaId).Select(m => new { m.ModeloId, m.Nombre, m.MarcaId }).ToListAsync();
            return Json(list);
        }


        // GET: InformacionHardware
        public async Task<ActionResult> Index()
        {
            var informacionHardwares = db.InformacionesHardware.Include(i => i.Equipo).Include(i => i.Fabricante).Include(i => i.Marca).Include(i => i.Modelo);
            return View(await informacionHardwares.ToListAsync());
        }

        // GET: InformacionHardwar/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InformacionHardware informacionHardware = await db.InformacionesHardware.FindAsync(id);
            if (informacionHardware == null)
            {
                return HttpNotFound();
            }
            return View(informacionHardware);
        }

        // GET: InformacionHardwar/Create
        public ActionResult Create()
        {
            ViewBag.EquipoId = new SelectList(db.Equipos, "EquipoId", "NombreCompleto");
            ViewBag.FabricanteId = new SelectList(db.Fabricantes, "FabricanteId", "Nombre");
            ViewBag.MarcaId = new SelectList(Enumerable.Empty<Marca>(), "MarcaId", "Nombre");
            ViewBag.ModeloId = new SelectList(Enumerable.Empty<Modelo>(), "ModeloId", "Nombre");
            return View();
        }

        // POST: InformacionHardwar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "EquipoId,NumeroSerie,FabricanteId,MarcaId,ModeloId")] InformacionHardware informacionHardware)
        {
            if (ModelState.IsValid)
            {
                db.InformacionesHardware.Add(informacionHardware);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EquipoId = new SelectList(db.Equipos, "EquipoId", "NombreCompleto", informacionHardware.EquipoId);
            ViewBag.FabricanteId = new SelectList(db.Fabricantes, "FabricanteId", "Nombre", informacionHardware.FabricanteId);
            ViewBag.MarcaId = new SelectList(db.Marcas, "MarcaId", "Nombre", informacionHardware.MarcaId);
            ViewBag.ModeloId = new SelectList(db.Modelos, "ModeloId", "Nombre", informacionHardware.ModeloId);
            return View(informacionHardware);
        }

        // GET: InformacionHardwar/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InformacionHardware informacionHardware = await db.InformacionesHardware.FindAsync(id);
            if (informacionHardware == null)
            {
                return HttpNotFound();
            }
            ViewBag.EquipoId = new SelectList(db.Equipos, "EquipoId", "NombreCompleto", informacionHardware.EquipoId);
            ViewBag.FabricanteId = new SelectList(db.Fabricantes, "FabricanteId", "Nombre", informacionHardware.FabricanteId);
            ViewBag.MarcaId = new SelectList(db.Marcas, "MarcaId", "Nombre", informacionHardware.MarcaId);
            ViewBag.ModeloId = new SelectList(db.Modelos, "ModeloId", "Nombre", informacionHardware.ModeloId);
            return View(informacionHardware);
        }

        // POST: InformacionHardwar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "EquipoId,NumeroSerie,FabricanteId,MarcaId,ModeloId")] InformacionHardware informacionHardware)
        {
            if (ModelState.IsValid)
            {
                db.Entry(informacionHardware).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EquipoId = new SelectList(db.Equipos, "EquipoId", "NombreCompleto", informacionHardware.EquipoId);
            ViewBag.FabricanteId = new SelectList(db.Fabricantes, "FabricanteId", "Nombre", informacionHardware.FabricanteId);
            ViewBag.MarcaId = new SelectList(db.Marcas, "MarcaId", "Nombre", informacionHardware.MarcaId);
            ViewBag.ModeloId = new SelectList(db.Modelos, "ModeloId", "Nombre", informacionHardware.ModeloId);
            return View(informacionHardware);
        }

        // GET: InformacionHardwar/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InformacionHardware informacionHardware = await db.InformacionesHardware.FindAsync(id);
            if (informacionHardware == null)
            {
                return HttpNotFound();
            }
            return View(informacionHardware);
        }

        // POST: InformacionHardwar/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InformacionHardware informacionHardware = await db.InformacionesHardware.FindAsync(id);
            db.InformacionesHardware.Remove(informacionHardware);
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
