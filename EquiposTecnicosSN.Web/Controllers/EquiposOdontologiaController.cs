using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Entities.Equipos;
using EquiposTecnicosSN.Entities.Equipos.Info;
using System.Data.Entity.Infrastructure;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class EquiposOdontologiaController : EquiposBaseController
    {

        // GET: EquiposClimatizacion/Create
        public ActionResult Create()
        {
            var model = new EquipoOdontologia();
            model.InformacionComercial = new InformacionComercial();
            model.InformacionHardware = new InformacionHardware();
            base.SetViewBagValues(model);
            return View(model);
        }

        // POST: EquiposClimatizacion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(EquipoOdontologia equipoCirugia)
        {
            if (EquipoDuplicado(equipoCirugia))
            {
                ModelState.AddModelError("", "Ya se encuentra ingresado un equipo de la misma marca y modelo con el nº de serie ingresado");
                base.SetViewBagValues(equipoCirugia);
                return View(equipoCirugia);
            }

            if (ModelState.IsValid)//validaciones
            {
                db.EquiposDeOdontologia.Add(equipoCirugia);
                db.SaveChanges();
                ViewBag.CssClass = "success";
                ViewBag.Message = "Equipo creado.";

                return RedirectToAction("Index", "EquiposBase");
            }

            base.SetViewBagValues(equipoCirugia);
            return View(equipoCirugia);
        }

        // GET: EquiposClimatizacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var equipoCirugia = db.EquiposDeOdontologia.Find(id);
            if (equipoCirugia == null)
            {
                return HttpNotFound();
            }
            base.SetViewBagValues(equipoCirugia);

            return View(equipoCirugia);
        }

        // POST: EquiposClimatizacion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(EquipoOdontologia equipoCirugia)
        {

            if (ModelState.IsValid) //validaciones
            {
                db.Entry(equipoCirugia).State = EntityState.Modified;
                db.Entry(equipoCirugia.InformacionComercial).State = EntityState.Modified;
                db.Entry(equipoCirugia.InformacionHardware).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "EquiposBase");
            }
            base.SetViewBagValues(equipoCirugia);
            return View(equipoCirugia);
        }

        // GET: EquiposClimatizacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var equipoCirugia = db.EquiposDeOdontologia.Find(id);
            if (equipoCirugia == null)
            {
                return HttpNotFound();
            }
            return View(equipoCirugia);
        }

        // POST: EquiposClimatizacion/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            EquipoOdontologia equipo = db.EquiposDeOdontologia.Find(id);
            db.EquiposDeOdontologia.Remove(equipo);
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
