using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EquiposTecnicosSN.Web.DataContexts;
using EquiposTecnicosSN.Entities.Equipos;
using EquiposTecnicosSN.Entities.Equipos.Info;
using PagedList;

namespace EquiposTecnicosSN.Web.Controllers
{
    public class EquiposBaseController : Controller
    {
        protected EquiposDbContext db = new EquiposDbContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult AutocompleteMarca(string term)
        {
            var model = db.Marcas.Where(m => m.Nombre.Contains(term))
                .Take(6)
                .Select(m => new
                {
                    label = m.Nombre,
                    value = m.MarcaId
                });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult AutocompleteModelo(string term)
        {
            var model = db.Modelos.Where(m => m.Nombre.Contains(term))
                .Take(6)
                .Select(m => new
                {
                    label = m.Nombre,
                    value = m.ModeloId
                });

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        // GET: EquiposBase
        public virtual ActionResult Index()
        {
            ViewBag.UbicacionId = new SelectList(db.Ubicaciones.OrderBy(u => u.Nombre), "UbicacionId", "Nombre");
            ViewBag.SectorId = new SelectList(db.Sectores.OrderBy(u => u.Nombre), "SectorId", "Nombre");
            return View(db.Equipos.ToList());
        }

        // GET
        public ActionResult AutocompleteNombreUMDNS(string term)
        {
            var model =
                db.Umdns
                .Where(u => u.NombreCompleto.Contains(term))
                .Take(6)
                .Select(e => new
                {
                    label = e.NombreCompleto,
                    value = e.Codigo
                });

            return Json(model, JsonRequestBehavior.AllowGet);

        }

        // GET
        public ActionResult AutocompleteCodigoUMDNS(string term)
        {
            var model =
                   db.Umdns
                   .Where(u => u.Codigo.StartsWith(term))
                   .Take(6)
                   .Select(e => new
                   {
                       label = e.Codigo,
                       value = e.NombreCompleto
                   });

            return Json(model, JsonRequestBehavior.AllowGet);

        }


        public void SetViewBagValues(Equipo equipo)
        {
            if (equipo.EquipoId == 0)
            {
                ViewBag.UbicacionId = new SelectList(db.Ubicaciones.OrderBy(u => u.Nombre), "UbicacionId", "Nombre");
                ViewBag.SectorId = new SelectList(db.Sectores.OrderBy(u => u.Nombre), "SectorId", "Nombre");
                ViewBag.ProveedorId = new SelectList(db.Proveedores.OrderBy(u => u.Nombre), "ProveedorId", "Nombre");
                ViewBag.FabricanteId = new SelectList(db.Fabricantes.OrderBy(u => u.Nombre), "FabricanteId", "Nombre");
                ViewBag.MarcaId = new SelectList(Enumerable.Empty<Marca>(), "MarcaId", "Nombre");
                ViewBag.ModeloId = new SelectList(Enumerable.Empty<Modelo>(), "ModeloId", "Nombre");
            }
            else
            {
                ViewBag.FabricanteId = new SelectList(db.Fabricantes.OrderBy(u => u.Nombre), "FabricanteId", "Nombre", equipo.InformacionHardware.FabricanteId);
                ViewBag.MarcaId = new SelectList(db.Marcas.OrderBy(u => u.Nombre).Where(m => m.FabricanteId == equipo.InformacionHardware.FabricanteId), "MarcaId", "Nombre", equipo.InformacionHardware.MarcaId);
                ViewBag.ModeloId = new SelectList(db.Modelos.Where(m => m.MarcaId == equipo.InformacionHardware.MarcaId), "ModeloId", "Nombre", equipo.InformacionHardware.ModeloId);
                ViewBag.UbicacionId = new SelectList(db.Ubicaciones.OrderBy(u => u.Nombre), "UbicacionId", "Nombre", equipo.UbicacionId);
                ViewBag.SectorId = new SelectList(db.Sectores.OrderBy(u => u.Nombre), "SectorId", "Nombre", equipo.SectorId);
                ViewBag.ProveedorId = new SelectList(db.Proveedores.OrderBy(u => u.Nombre), "ProveedorId", "Nombre", equipo.InformacionComercial.ProveedorId);
            }
        }

        // GET: EquiposBase/Details/5
        public virtual ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipo equipoBase = db.Equipos.Find(id);
            if (equipoBase == null)
            {
                return HttpNotFound();
            }
            return View(equipoBase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buscarNombreCompleto"></param>
        /// <param name="buscarUMDNS"></param>
        /// <param name="UbicacionId"></param>
        /// <param name="SectorId"></param>
        /// <param name="Estado"></param>
        /// <param name="NumeroMatricula"></param>
        /// <returns></returns>
        public ActionResult SearchEquipos(int? UbicacionId, int? SectorId, int? SearchTipoEquipo = 0, string NumeroSerie = "", string buscarMarca = "", string buscarModelo = "", int page = 1, int pageSize = 25)
        {

            var result = db.Equipos
                .Where(e => buscarMarca.Equals("") || e.InformacionHardware.Marca.Nombre.Contains(buscarMarca))
                .Where(e => buscarModelo.Equals("") || e.InformacionHardware.Modelo.Nombre.Contains(buscarModelo))
                .Where(e => NumeroSerie.Equals("") || e.InformacionHardware.NumeroSerie.Contains(NumeroSerie))
                .Where(e => UbicacionId == null || e.UbicacionId == UbicacionId)
                .Where(e => SectorId == null || e.SectorId == SectorId);
            //.Where(e => NumeroMatricula == null || e.NumeroMatricula.Equals(NumeroMatricula));

            if (SearchTipoEquipo != 0)
            {
                TipoEquipo tipo = (TipoEquipo)SearchTipoEquipo;

                switch (tipo)
                {
                    case TipoEquipo.Cirugia:
                        result = result.Where(e => e is EquipoCirugia);
                        break;

                    case TipoEquipo.Climatizacion:
                        result = result.Where(e => e is EquipoClimatizacion);
                        break;

                    case TipoEquipo.Edilicio:
                        result = result.Where(e => e is EquipamientoEdilicio);
                        break;

                    case TipoEquipo.Endoscopia:
                        result = result.Where(e => e is EquipoEndoscopia);
                        break;

                    case TipoEquipo.GasesMedicinales:
                        result = result.Where(e => e is EquipoGasesMedicinales);
                        break;

                    case TipoEquipo.Imagenes:
                        result = result.Where(e => e is EquipoImagen);
                        break;

                    case TipoEquipo.Informatica:
                        result = result.Where(e => e is EquipoInformatica);
                        break;

                    case TipoEquipo.Luces:
                        result = result.Where(e => e is EquipoLuces);
                        break;

                    case TipoEquipo.Monitoreo:
                        result = result.Where(e => e is EquipoMonitoreo);
                        break;

                    case TipoEquipo.Odontologia:
                        result = result.Where(e => e is EquipoOdontologia);
                        break;

                    case TipoEquipo.Otros:
                        result = result.Where(e => e is EquipoOtro);
                        break;

                    case TipoEquipo.PruebasDeDiagnostico:
                        result = result.Where(e => e is EquipoPruebaDeDiagnostico);
                        break;

                    case TipoEquipo.Rehabilitacion:
                        result = result.Where(e => e is EquipoRehabilitacion);
                        break;

                    case TipoEquipo.SoporteDeVida:
                        result = result.Where(e => e is EquipoSoporteDeVida);
                        break;

                    case TipoEquipo.Terapeutica:
                        result = result.Where(e => e is EquipoTerapeutica);
                        break;
                }
            }


            return PartialView("_SearchEquiposResults", result.OrderByDescending(e => e.NombreCompleto).ToPagedList(page, pageSize));
        }

        //GET: Generar código QR para equipo e imprimir.
        public virtual ActionResult GenerarQR(int? equipoId)
        {
            var model = db.Equipos.Find(equipoId);
            return PartialView("_GenerarQRContent", model);
        }

        protected bool EquipoDuplicado(Equipo equipo)
        {
            var count = db.Equipos.Where(e => e.InformacionHardware.MarcaId == equipo.InformacionHardware.MarcaId)
                    .Where(e => e.InformacionHardware.ModeloId == equipo.InformacionHardware.ModeloId)
                    .Where(e => e.InformacionHardware.NumeroSerie == equipo.InformacionHardware.NumeroSerie)
                    .Count();
            return count != 0;
        }

        public ActionResult BajaEquipo(int id)
        {
            var equipo = db.Equipos.Find(id);
            equipo.Estado = EstadoDeEquipo.NoFuncional;
            db.Entry(equipo).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
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
