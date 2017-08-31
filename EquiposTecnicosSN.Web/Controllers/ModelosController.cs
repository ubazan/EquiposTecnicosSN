using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Web.DataContexts;
using EquiposTecnicosSN.Entities.Equipos.Info;
using System.Linq;
using PagedList;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class ModelosController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();

        // GET: Modelos
        public ActionResult Index(int marcaId, string searchNombre = null, int page = 1)
        {
            var listPage = db.Modelos
                .Where(m => m.MarcaId == marcaId)
                .Where(m => (searchNombre == null || searchNombre == "") || m.Nombre.Contains(searchNombre))
                .OrderBy(m => m.Nombre)
                .ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("ModelosList", listPage);
            }

            ViewBag.MarcaId = marcaId;
            ViewBag.NombreMarca = db.Marcas.Where(m => m.MarcaId == marcaId).Select(f => f.Nombre).Single();
            ViewBag.FabricanteId = db.Marcas.Where(m => m.MarcaId == marcaId).Select(f => f.FabricanteId).Single();
            return View(listPage);
        }

        // GET: Modelos/Create
        public ActionResult Create(int marcaId)
        {
            ViewBag.MarcaId = new SelectList(db.Marcas, "MarcaId", "Nombre");
            var modelo = new Modelo
            {
                MarcaId = marcaId
            };
            return View(modelo);
        }

        // POST: Modelos/Create
        [HttpPost]
        public ActionResult Create(Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                db.Modelos.Add(modelo);
                db.SaveChanges();
                return RedirectToAction("Index", new { marcaId = modelo.MarcaId});
            }

            ViewBag.MarcaId = new SelectList(db.Marcas, "MarcaId", "Nombre", modelo.MarcaId);
            return View(modelo);
        }

        // GET: Modelos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = db.Modelos.Find(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            ViewBag.MarcaId = new SelectList(db.Marcas, "MarcaId", "Nombre", modelo.MarcaId);
            return View(modelo);
        }

        // POST: Modelos/Edit/5
        [HttpPost]
        public ActionResult Edit(Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { marcaId = modelo.MarcaId });
            }
            ViewBag.MarcaId = new SelectList(db.Marcas, "MarcaId", "Nombre", modelo.MarcaId);
            return View(modelo);
        }

        // GET: Modelos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = db.Modelos.Find(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            return View(modelo);
        }

        // POST: Modelos/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var equiposCount = db.Equipos
                .Where(e => e.InformacionHardware.ModeloId == id)
                .Count();

            var modelo = db.Modelos.Find(id);

            if (equiposCount == 0)
            {
                ViewBag.MarcaId = modelo.MarcaId;
                ViewBag.FabricanteId = modelo.Marca.FabricanteId;
                db.Modelos.Remove(modelo);
                db.SaveChanges();
                return RedirectToAction("Index", new { marcaId = ViewBag.MarcaId });
            }

            ModelState.AddModelError("", "El Modelo no puede eliminarse ya que posee equipos asociados.");
            return View(modelo);
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
