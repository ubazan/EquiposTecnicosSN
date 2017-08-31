using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Entities.Equipos.Info;
using EquiposTecnicosSN.Web.DataContexts;
using PagedList;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class UmdnsController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();

        // GET: Umdns
        public ActionResult Index(string searchUmdns = null, int page = 1 )
        {
            var listPage = db.Umdns
                .Where(umdns => searchUmdns == null || umdns.NombreCompleto.Contains(searchUmdns))
                .OrderBy(umdns => umdns.NombreCompleto)
                .ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_UmdnsList", listPage);
            }

            return View(listPage);
        }

        // GET: Umdns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umdns umdns = db.Umdns.Find(id);
            if (umdns == null)
            {
                return HttpNotFound();
            }
            return View(umdns);
        }

        // GET: Umdns/Create
        public ActionResult Create()
        {
            return View(new Umdns());
        }

        // POST: Umdns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "UmdnsId,Codigo,NombreCompleto")] Umdns umdns)
        {
            if (ModelState.IsValid)
            {
                db.Umdns.Add(umdns);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(umdns);
        }

        // GET: Umdns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umdns umdns = db.Umdns.Find(id);
            if (umdns == null)
            {
                return HttpNotFound();
            }
            return View(umdns);
        }

        // POST: Umdns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "UmdnsId,Codigo,NombreCompleto")] Umdns umdns)
        {
            if (ModelState.IsValid)
            {
                db.Entry(umdns).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(umdns);
        }

        // GET: Umdns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umdns umdns = db.Umdns.Find(id);
            if (umdns == null)
            {
                return HttpNotFound();
            }
            return View(umdns);
        }

        // POST: Umdns/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Umdns umdns = db.Umdns.Find(id);
            db.Umdns.Remove(umdns);
            db.SaveChanges();
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
