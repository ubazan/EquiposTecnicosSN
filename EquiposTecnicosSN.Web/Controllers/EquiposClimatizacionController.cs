using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Entities.Equipos;
using EquiposTecnicosSN.Entities.Equipos.Info;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class EquiposClimatizacionController : EquiposBaseController
    {
        // GET: EquiposClimatizacion/Create
        public ActionResult Create()
        {
            var model = new EquipoClimatizacion();
            model.InformacionComercial = new InformacionComercial();
            model.InformacionHardware = new InformacionHardware();
            base.SetViewBagValues(model);
            return View(model);
        }

        // POST: EquiposClimatizacion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(EquipoClimatizacion equipo)
        {
            if (EquipoDuplicado(equipo))
            {
                ModelState.AddModelError("", "Ya se encuentra ingresado un equipo de la misma marca y modelo con el nº de serie ingresado");
                base.SetViewBagValues(equipo);
                return View(equipo);
            }
            if (ModelState.IsValid)//validaciones
            {
                db.EquiposDeClimatizacion.Add(equipo);
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
            EquipoClimatizacion equipoClimatizacion = db.EquiposDeClimatizacion.Find(id);
            if (equipoClimatizacion == null)
            {
                return HttpNotFound();
            }
            base.SetViewBagValues(equipoClimatizacion);

            return View(equipoClimatizacion);
        }

        // POST: EquiposClimatizacion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "EquipoId,NombreCompleto,UMDNS,Tipo,NumeroMatricula,NumeroInventario,UbicacionId,Estado,InformacionComercial,InformacionHardware")] EquipoClimatizacion equipoClimatizacion)
        {

            if (ModelState.IsValid) //validaciones
            {
                db.Entry(equipoClimatizacion).State = EntityState.Modified;
                db.Entry(equipoClimatizacion.InformacionComercial).State = EntityState.Modified;
                db.Entry(equipoClimatizacion.InformacionHardware).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "EquiposBase");
            }
            base.SetViewBagValues(equipoClimatizacion);
            return View(equipoClimatizacion);
        }

        // GET: EquiposClimatizacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipoClimatizacion equipoClimatizacion = db.EquiposDeClimatizacion.Find(id);
            if (equipoClimatizacion == null)
            {
                return HttpNotFound();
            }
            return View(equipoClimatizacion);
        }

        // POST: EquiposClimatizacion/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            EquipoClimatizacion equipoClimatizacion = db.EquiposDeClimatizacion.Find(id);
            db.EquiposDeClimatizacion.Remove(equipoClimatizacion);
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
