using EquiposTecnicosSN.Entities.Mantenimiento;
using EquiposTecnicosSN.Web.DataContexts;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class StockRepuestosController : Controller
    {
        EquiposDbContext db = new EquiposDbContext();
        // GET: StockRepuestos
        public ActionResult Index(string searchCodigoRepuesto = null, int repuestoId = 0, int proveedorId = 0, int page = 1)
        {
            var listPage = db.StockRepuestos
                .Where(sr => searchCodigoRepuesto == null || sr.Repuesto.Codigo.Contains(searchCodigoRepuesto))
                .Where(sr => proveedorId == 0 || sr.Repuesto.ProveedorId == proveedorId)
                .Where(sr => repuestoId == 0 || sr.Repuesto.RepuestoId == repuestoId)
                .OrderBy(sr => sr.Repuesto.Nombre)
                .ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_StockRepuestosList", listPage);
            }

            ViewBag.proveedorId = new SelectList(db.Proveedores, "ProveedorId", "Nombre");
            ViewBag.repuestoId = new SelectList(db.Repuestos, "RepuestoId", "Nombre");
            return View(listPage);
        }

        public ActionResult Create()
        {
            ViewBag.RepuestoId = new SelectList(db.Repuestos, "RepuestoId", "Nombre");
            return View(new StockRepuesto());
        }

        // POST: StockRepuestos/Create
        [HttpPost]
        public ActionResult Create(StockRepuesto stock)
        {
            bool stockExistente = StockRepuestoExistente(stock.RepuestoId);

            if (ModelState.IsValid && !stockExistente)
            {
                db.StockRepuestos.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (stockExistente)
            {
                ModelState.AddModelError("", "Ya existe un stock asociado a este repuesto.");
            }

            ViewBag.RepuestoId = new SelectList(db.Repuestos, "RepuestoId", "Nombre", stock.RepuestoId);
            return View(stock);
        }

        // GET: StockRepuestos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var stock = db.StockRepuestos.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.RepuestoId = new SelectList(db.Repuestos, "RepuestoId", "Nombre", stock.RepuestoId);
            return View(stock);
        }

        // POST: StockRepuestos/Edit/5
        [HttpPost]
        public ActionResult Edit(StockRepuesto stock)
        {
            bool stockExistente = StockRepuestoExistente(stock.RepuestoId);

            if (ModelState.IsValid && !stockExistente)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (stockExistente)
            {
                ModelState.AddModelError("", "Ya existe un stock asociado a este repuesto.");
            }

            ViewBag.RepuestoId = new SelectList(db.Proveedores, "ProveedorId", "Nombre", stock.RepuestoId);
            return View(stock);
        }

        // GET: StockRepuestos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var stock = db.StockRepuestos.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: StockRepuestos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var stock = db.StockRepuestos.Find(id);
                db.StockRepuestos.Remove(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private bool StockRepuestoExistente(int repuestoId)
        {
            var stockDeRepuesto = db.StockRepuestos
            .Where(sr => sr.RepuestoId == repuestoId)
            .Count();

            return stockDeRepuesto > 0;
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
