using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Web.DataContexts;
using EquiposTecnicosSN.Entities.Equipos.Info;
using PagedList;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class UbicacionesController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();

        // GET: Ubicaciones
        public ActionResult Index(string searchUbicacion = null, int page = 1)
        {
            var listPage = db.Ubicaciones
                .Where(u => searchUbicacion == null || u.Nombre.Contains(searchUbicacion))
                .OrderBy(u => u.Nombre)
                .ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_UbicacionesList", listPage);
            }

            return View(listPage);
        }

        // GET: Ubicaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ubicacion ubicacion = db.Ubicaciones.Find(id);
            if (ubicacion == null)
            {
                return HttpNotFound();
            }
            return View(ubicacion);
        }

        // GET: Ubicaciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ubicaciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "UbicacionId,Nombre")] Ubicacion ubicacion)
        {
            if (ModelState.IsValid)
            {
                db.Ubicaciones.Add(ubicacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ubicacion);
        }

        // GET: Ubicaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ubicacion ubicacion = db.Ubicaciones.Find(id);
            if (ubicacion == null)
            {
                return HttpNotFound();
            }
            return View(ubicacion);
        }

        // POST: Ubicaciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "UbicacionId,Nombre")] Ubicacion ubicacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ubicacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ubicacion);
        }

        // GET: Ubicaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ubicacion ubicacion = db.Ubicaciones.Find(id);
            if (ubicacion == null)
            {
                return HttpNotFound();
            }
            return View(ubicacion);
        }

        // POST: Ubicaciones/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var equiposCount = db.Equipos
                .Where(e => e.UbicacionId == id)
                .Count();

            var ubicacion = db.Ubicaciones.Find(id);

            if (equiposCount == 0)
            {
                db.Ubicaciones.Remove(ubicacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "La ubicación no puede eliminarse ya que posee equipos asociados.");
            return View(ubicacion);
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
