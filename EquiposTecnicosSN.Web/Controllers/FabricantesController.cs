using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Web.DataContexts;
using EquiposTecnicosSN.Entities.Equipos.Info;
using System.Linq;
using PagedList;
using System.Collections.Generic;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class FabricantesController : Controller
    {
        private EquiposDbContext db = new EquiposDbContext();

        // GET: Fabricantes
        public ActionResult Index(string searchNombre = null, string searchMarca = null, string searchModelo = null, int page = 1)
        {
            int? fabricanteId = null;
            List<int> fabricantesId = new List<int>();
            bool checkFabricanteId = false;
            bool checkFabricantesIds = false;
            if (searchMarca != null && searchMarca != "")
            {
                var marcas = db.Marcas.Where(m => m.Nombre.Contains(searchMarca)).ToList();
                if  (marcas.Count == 1)
                {
                    checkFabricanteId = true;
                    fabricanteId = marcas.First().FabricanteId;
                } else if (marcas.Count > 1)
                {
                    checkFabricantesIds = true;
                    fabricantesId = marcas.Select(m => m.FabricanteId).ToList();
                } else if (marcas.Count == 0)
                {
                    checkFabricanteId = true;
                    checkFabricantesIds = true;
                }

            }

            if (searchModelo != null && searchModelo != "")
            {
                var modelos = db.Modelos.Where(mod => mod.Nombre.Contains(searchModelo)).ToList();
                if (modelos.Count == 1)
                {
                    checkFabricanteId = true;
                    fabricanteId = modelos.First().Marca.FabricanteId;
                } else if (modelos.Count > 1)
                {
                    checkFabricantesIds = true;
                    fabricantesId = modelos.Select(m => m.Marca.FabricanteId).ToList();
                }
                else if (modelos.Count == 0)
                {
                    checkFabricanteId = true;
                    checkFabricantesIds = true;
                }
            }

            var listPage = db.Fabricantes
                .Where(f => (searchNombre == null || searchNombre == "") || f.Nombre.Contains(searchNombre))
                .Where(f => (!checkFabricanteId && fabricanteId == null) || f.FabricanteId == fabricanteId)
                .Where(f => (!checkFabricantesIds && fabricantesId.Count == 0) || fabricantesId.Contains(f.FabricanteId))
                .OrderBy(f => f.Nombre)
                .ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_FabricantesList", listPage);
            }

            return View(listPage);

        }

        // GET: Fabricantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fabricante fabricante = db.Fabricantes.Find(id);
            if (fabricante == null)
            {
                return HttpNotFound();
            }
            return View(fabricante);
        }

        // GET: Fabricantes/Create
        public ActionResult Create()
        {
            return View(new Fabricante());
        }

        // POST: Fabricantes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Fabricante fabricante)
        {
            if (ModelState.IsValid)
            {
                db.Fabricantes.Add(fabricante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fabricante);
        }

        // GET: Fabricantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fabricante fabricante = db.Fabricantes.Find(id);
            if (fabricante == null)
            {
                return HttpNotFound();
            }
            return View(fabricante);
        }

        // POST: Fabricantes/Edit/5
        [HttpPost]
        public ActionResult Edit(Fabricante fabricante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fabricante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fabricante);
        }

        // GET: Fabricantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fabricante fabricante = db.Fabricantes.Find(id);
            if (fabricante == null)
            {
                return HttpNotFound();
            }
            return View(fabricante);
        }

        // POST: Fabricantes/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var equiposCount = db.Equipos
                .Where(e => e.InformacionHardware.FabricanteId == id)
                .Count();

            var marcasCount = db.Marcas
                .Where(m => m.FabricanteId == id)
                .Count();

            var fabricante = db.Fabricantes.Find(id);

            if (equiposCount == 0 && marcasCount == 0)
            {                
                db.Fabricantes.Remove(fabricante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (equiposCount > 0)
            {
                ModelState.AddModelError("", "El Fabricante no puede eliminarse ya que posee equipos asociados.");
            }

            if (marcasCount > 0)
            {
                ModelState.AddModelError("", "El Fabricante no puede eliminarse ya que posee Marcas asociados.");
            }

            return View(fabricante);
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
