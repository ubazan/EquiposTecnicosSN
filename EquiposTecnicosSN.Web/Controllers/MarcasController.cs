using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Web.DataContexts;
using EquiposTecnicosSN.Entities.Equipos.Info;
using System.Linq;
using PagedList;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class MarcasController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();

        // GET: Marcas
        public ActionResult Index(int fabricanteId, string searchNombre = null, int page = 1)
        {
            var listPage = db.Marcas
                .Where(m => m.FabricanteId == fabricanteId)
                .Where(m => (searchNombre == null || searchNombre == "") || m.Nombre.Contains(searchNombre))
                .OrderBy(m => m.Nombre)
                .ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_MarcasList", listPage);
            }

            ViewBag.FabricanteId = fabricanteId;
            ViewBag.NombreFabricante = db.Fabricantes.Where(f => f.FabricanteId == fabricanteId).Select(f => f.Nombre).Single();
            return View(listPage);
        }

        // GET: Marcas/Create
        public ActionResult Create(int fabricanteId)
        {
            ViewBag.FabricanteId = new SelectList(db.Fabricantes, "FabricanteId", "Nombre");
            var nuevaMarca = new Marca
            {
                FabricanteId = fabricanteId
            };
            return View(nuevaMarca);
        }

        // POST: Marcas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Marca marca)
        {
            if (ModelState.IsValid)
            {
                db.Marcas.Add(marca);
                db.SaveChanges();
                return RedirectToAction("Index", new { fabricanteId = marca.FabricanteId});
            }

            ViewBag.FabricanteId = new SelectList(db.Fabricantes, "FabricanteId", "Nombre", marca.FabricanteId);
            return View(marca);
        }

        // GET: Marcas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            ViewBag.FabricanteId = new SelectList(db.Fabricantes, "FabricanteId", "Nombre", marca.FabricanteId);
            return View(marca);
        }

        // POST: Marcas/Edit/5
        [HttpPost]
        public ActionResult Edit(Marca marca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { fabricanteId = marca.FabricanteId });
            }
            ViewBag.FabricanteId = new SelectList(db.Fabricantes, "FabricanteId", "Nombre", marca.FabricanteId);
            return View(marca);
        }

        // GET: Marcas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var equiposCount = db.Equipos
                .Where(e => e.InformacionHardware.MarcaId == id)
                .Count();

            var modelosCount = db.Modelos
                .Where(m => m.MarcaId == id)
                .Count();

            var marca = db.Marcas.Find(id);

            if (equiposCount == 0 && modelosCount == 0)
            {
                ViewBag.FabricanteId = marca.FabricanteId;
                db.Marcas.Remove(marca);
                db.SaveChanges();
                return RedirectToAction("Index", new { fabricanteId = ViewBag.FabricanteId });
            }

            if (equiposCount > 0)
            {
                ModelState.AddModelError("", "La Marca no puede eliminarse ya que posee equipos asociados.");
            }

            if (modelosCount > 0)
            {
                ModelState.AddModelError("", "La Marca no puede eliminarse ya que posee Modelos asociados.");
            }

            return View(marca);
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
