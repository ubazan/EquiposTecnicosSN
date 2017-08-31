using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Web.DataContexts;
using EquiposTecnicosSN.Entities.Mantenimiento;
using PagedList;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class ProveedoresController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();

        // GET: Proveedores
        public ActionResult Index(string searchNombre = null, string searchServicios = null, int searchTipo = 0, int page = 1)
        {
            var listPage = db.Proveedores
                .Where(p => (searchNombre == "" || searchNombre == null) || p.Nombre.Contains(searchNombre))
                .Where(p => (searchServicios == "" || searchServicios == null) || p.Servicios.Contains(searchServicios))
                .Where(r => searchTipo == 0 || (int)r.Tipo == searchTipo)
                .OrderBy(r => r.Nombre)
                .ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProveedoresList", listPage);
            }

            return View(listPage);
        }

        // GET: Proveedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // GET: Proveedores/Create
        public ActionResult Create()
        {
            return View(new Proveedor());
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Proveedores.Add(proveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(proveedor);
        }

        // GET: Proveedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit( Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proveedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            var equiposCount = db.Equipos
                .Where(e => e.InformacionComercial.ProveedorId == id)
                .Count();

            var repuestosCount = db.Repuestos
                .Where(r => r.ProveedorId == id)
                .Count();

            var solicitudesCount = db.SolicitudesRepuestosServicios
                .Where(sr => sr.ProveedorId == id)
                .Count();
            
            var proveedor = db.Proveedores.Find(id);

            if (equiposCount == 0 && repuestosCount == 0 && solicitudesCount == 0)
            {
                db.Proveedores.Remove(proveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (equiposCount != 0)
            {
                ModelState.AddModelError("", "El proveedor no puede eliminarse ya que posee equipos asociados.");
            }

            if (repuestosCount != 0)
            {
                ModelState.AddModelError("", "El proveedor no puede eliminarse ya que posee repuestos asociados.");
            }

            if (solicitudesCount != 0)
            {
                ModelState.AddModelError("", "El proveedor no puede eliminarse ya que posee solicitudes asociadas.");
            }

            return View(proveedor);
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
