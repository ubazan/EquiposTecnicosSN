using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Entities.Equipos;
using EquiposTecnicosSN.Entities.Equipos.Info;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class EquiposImagenesController : EquiposBaseController
    {

        // GET: EquiposClimatizacion/Create
        public ActionResult Create()
        {
            var model = new EquipoImagen();
            model.InformacionComercial = new InformacionComercial();
            model.InformacionHardware = new InformacionHardware();
            base.SetViewBagValues(model);
            return View(model);
        }

        // POST: EquiposClimatizacion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(EquipoImagen equipo)
        {
            if (EquipoDuplicado(equipo))
            {
                ModelState.AddModelError("", "Ya se encuentra ingresado un equipo de la misma marca y modelo con el nº de serie ingresado");
                base.SetViewBagValues(equipo);
                return View(equipo);
            }
            
            if (ModelState.IsValid)//validaciones
            {
                db.EquiposDeImagenes.Add(equipo);
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
            var equipo = db.EquiposDeImagenes.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            base.SetViewBagValues(equipo);

            return View(equipo);
        }

        // POST: EquiposClimatizacion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(EquipoImagen equipo)
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
            var equipo = db.EquiposDeImagenes.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        // POST: EquiposClimatizacion/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            EquipoImagen equipo = db.EquiposDeImagenes.Find(id);
            db.EquiposDeImagenes.Remove(equipo);
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
