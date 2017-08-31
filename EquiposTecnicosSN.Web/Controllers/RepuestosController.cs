using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Entities.Mantenimiento;
using EquiposTecnicosSN.Web.DataContexts;
using PagedList;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class RepuestosController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();

        // GET
        public async Task<JsonResult> CheckStockRepuesto(string codigo, int cantidad)
        {
            var stock = await db.StockRepuestos.Where(s => s.Repuesto.Codigo == codigo).SingleOrDefaultAsync();

            var repuesto = await db.Repuestos.Where(r => r.Codigo == codigo).SingleOrDefaultAsync();

            var response = new {
                existeRepuesto = repuesto != null,
                hayStock = stock != null && stock.CantidadDisponible >= cantidad,
                proveedorId = (repuesto != null ? repuesto.ProveedorId : null)
            };

            return Json(response, JsonRequestBehavior.AllowGet);

        }


        // GET
        public ActionResult AutocompleteCodigoRepuesto(string term)
        {
            var model =
                db.Repuestos
                .Where(r => r.Codigo.Contains(term))
                .Take(10)
                .Select(r => new
                {
                    label = r.Codigo,
                    value = r.Nombre
                });

            return Json(model, JsonRequestBehavior.AllowGet);

        }

        // GET: Repuestos
        public ActionResult Index(string searchNombre = null, string searchCodigo = null, int proveedorId = 0, int page = 1)
        {
            var listPage = db.Repuestos.Include(r => r.Proveedor)
                .Where(r => searchNombre == null || r.Nombre.Contains(searchNombre))
                .Where(r => searchCodigo == null || r.Codigo.Contains(searchCodigo))
                .Where(r => proveedorId == 0 || r.ProveedorId == proveedorId)
                .OrderBy(r => r.Nombre)
                .ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_RepuestosList", listPage);
            }
            
            ViewBag.proveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre");
            return View(listPage);
        }

        // GET: Repuestos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repuesto repuesto = db.Repuestos.Find(id);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            return View(repuesto);
        }

        // GET: Repuestos/Create
        public ActionResult Create()
        {
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre");
            return View(new Repuesto());
        }

        // POST: Repuestos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create( Repuesto repuesto)
        {

            var codigoRepetido = CodigoRepetido(repuesto.Codigo);

            if (ModelState.IsValid && !codigoRepetido)
            {
                db.Repuestos.Add(repuesto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (codigoRepetido)
            {
                ModelState.AddModelError("", "Ya se ha registrado un repuesto con el código ingresado.");
            }

            ViewBag.ProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre", repuesto.ProveedorId);
            return View(repuesto);
        }

        // GET: Repuestos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repuesto repuesto = db.Repuestos.Find(id);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre", repuesto.ProveedorId);
            return View(repuesto);
        }

        // POST: Repuestos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Repuesto repuesto)
        {
            var codigoRepetido = CodigoRepetido(repuesto.Codigo);

            if (ModelState.IsValid && !codigoRepetido)
            {
                db.Entry(repuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (codigoRepetido)
            {
                ModelState.AddModelError("", "Ya se ha registrado un repuesto con el código ingresado.");
            }

            ViewBag.ProveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre", repuesto.ProveedorId);
            return View(repuesto);
        }

        // GET: Repuestos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repuesto repuesto = db.Repuestos.Find(id);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            return View(repuesto);
        }

        // POST: Repuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var stockCount = db.StockRepuestos
                .Where(sr => sr.RepuestoId == id)
                .Count();

            var solicitudesCount = db.SolicitudesRepuestosServicios
                .Where(sr => sr.RepuestoId == id)
                .Count();

            var equiposCount = db.InformacionesComerciales
                .Where(ic => ic.ProveedorId == id)
                .Count();

            var repuesto = db.Repuestos.Find(id);

            if (equiposCount == 0 && solicitudesCount == 0 && stockCount == 0)
            {
                db.Repuestos.Remove(repuesto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (equiposCount != 0)
            {
                ModelState.AddModelError("", "El repuesto no puede eliminarse ya que posee equipos asociados.");
            }

            if (solicitudesCount != 0)
            {
                ModelState.AddModelError("", "El repuesto no puede eliminarse ya que posee pedidos asociados.");
            }

            if (stockCount != 0)
            {
                ModelState.AddModelError("", "El repuesto no puede eliminarse ya que posee un stock asociado.");
            }

            return View(repuesto);
        }

        private bool CodigoRepetido(string codigo)
        {
            var repuestosMismoCodigoCount = db.Repuestos
                .Where(r => r.Codigo == codigo)
                .Count();

            return repuestosMismoCodigoCount > 0;
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
