using System.Data.Entity;
using System.Diagnostics;
using EquiposTecnicosSN.Entities.Equipos;
using EquiposTecnicosSN.Entities.Mantenimiento;
using EquiposTecnicosSN.Entities.Usuarios;
using EquiposTecnicosSN.Entities.Equipos.Info;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace EquiposTecnicosSN.Web.DataContexts
{

    public class EquiposDbContext : DbContext
    {
        public EquiposDbContext()
            : base("EquiposTecnicosDbContext")
        {
            Database.Log = log => Debug.Write(log);
        }

        public DbSet<SolicitudUsuario> SolicitudesUsuarios { get; set; }

        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<InformacionComercial> InformacionesComerciales { get; set; }
        public DbSet<InformacionHardware> InformacionesHardware { get; set; }
        public DbSet<Traslado> Traslados { get; set; }

        public DbSet<EquipoClimatizacion> EquiposDeClimatizacion { get; set; }
        public DbSet<EquipoCirugia> EquiposDeCirugia { get; set; }
        public DbSet<EquipoEndoscopia> EquiposDeEndoscopia { get; set; }
        public DbSet<EquipamientoEdilicio> EquipamientosEdilicios { get; set; }
        public DbSet<EquipoSoporteDeVida> EquiposDeSoporteDeVida { get; set; }
        public DbSet<EquipoGasesMedicinales> EquiposDeGasesMedicinales { get; set; }
        public DbSet<EquipoImagen> EquiposDeImagenes { get; set; }
        public DbSet<EquipoLuces> EquiposDeLuces { get; set; }
        public DbSet<EquipoMonitoreo> EquiposDeMonitoreo { get; set; }
        public DbSet<EquipoInformatica> EquiposDeInformatica { get; set; }
        public DbSet<EquipoOdontologia> EquiposDeOdontologia { get; set; }
        public DbSet<EquipoPruebaDeDiagnostico> EquiposDePruebasDeDiagnostico { get; set; }
        public DbSet<EquipoRehabilitacion> EquiposDeRehabilitacion { get; set; }
        public DbSet<EquipoTerapeutica> EquiposDeTerapeutica { get; set; }
        public DbSet<EquipoOtro> EquiposOtros { get; set; }

        public DbSet<OrdenDeTrabajo> OrdenesDeTrabajo { get; set; }
        public DbSet<OrdenDeTrabajoMantenimientoCorrectivo> ODTMantenimientosCorrectivos { get; set; }
        public DbSet<OrdenDeTrabajoMantenimientoPreventivo> ODTMantenimientosPreventivos { get; set; }
        public DbSet<GastoOrdenDeTrabajo> GastosOrdenesDeTrabajo { get; set; }
        public DbSet<SolicitudRepuestoServicio> SolicitudesRepuestosServicios { get; set; }
        public DbSet<ChecklistMantenimientoPreventivo> ChecklistsMantenimientoPreventivo { get; set; }
        public DbSet<ObservacionOrdenDeTrabajo> ObservacionesOrdenesDeTrabajo { get; set; }


        public DbSet<Repuesto> Repuestos { get; set; }
        public DbSet<StockRepuesto> StockRepuestos { get; set; }

        public DbSet<Umdns> Umdns { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Sector> Sectores { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Fabricante> Fabricantes { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Modelo> Modelos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            Database.SetInitializer<EquiposDbContext>(null);
        }
    }
}