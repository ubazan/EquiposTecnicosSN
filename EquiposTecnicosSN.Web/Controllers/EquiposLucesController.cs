using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Entities.Equipos;
using EquiposTecnicosSN.Entities.Equipos.Info;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class EquiposLucesController : EquiposBaseController
    {

        // GET: EquiposClimatizacion/Create
        public ActionResult Create()
        {
            var model = new EquipoLuces();
            model.InformacionComercial = new InformacionComercial();
            model.InformacionHardware = new InformacionHardware();
            base.SetViewBagValues(model);
            return View(model);
        }

        // POST: EquiposClimatizacion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(EquipoLuces equipo)
        {
            if (EquipoDuplicado(equipo))
            {
                ModelState.AddModelError("", "Ya se encuentra ingresado un equipo de la misma marca y modelo con el nº de serie ingresado");
                base.SetViewBagValues(equipo);
                return View(equipo);
            }

            if (ModelState.IsValid)//validaciones
            {
                db.EquiposDeLuces.Add(equipo);
                db.SaveChanges();
                ViewBag.CssClass = "success";
                ViewBag.Message = "Equipo creado.";

                return RedirectToAction("Index", "EquiposBase");
            }

            base.SetViewBagValues(equipo);
            return View(equipo);
        }

        // GET: EquiposClimatizacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var equipoCirugia = db.EquiposDeLuces.Find(id);
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
        public ActionResult Edit(EquipoLuces equipo)
        {

            if (ModelState.IsValid) //validaciones
            {
                db.Entry(equipo).State = EntityState.Modified;
                db.Entry(equipo.InformacionComercial).State = EntityState.Modified;
                db.Entry(equipo.InformacionHardware).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "EquiposBase");
            }
            base.SetViewBagValues(equipo);
            return View(equipo);
        }

        // GET: EquiposClimatizacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var equipoCirugia = db.EquiposDeLuces.Find(id);
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
            EquipoLuces equipo = db.EquiposDeLuces.Find(id);
            db.EquiposDeLuces.Remove(equipo);
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
