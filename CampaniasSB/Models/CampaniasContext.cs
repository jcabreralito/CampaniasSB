using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CampaniasSB.Models
{
    public class CampaniasContext : DbContext
    {
        public CampaniasContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        public DbSet<Compañia> Compañias { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Rol> Roles { get; set; }

        public DbSet<Tienda> Tiendas { get; set; }

        public DbSet<Ciudad> Ciudades { get; set; }

        public DbSet<Region> Regiones { get; set; }

        public DbSet<Campaña> Campañas { get; set; }

        public DbSet<NivelPrecio> NivelPrecios { get; set; }

        public DbSet<TipoCaja> TipoCajas { get; set; }

        public DbSet<AcomodoCaja> AcomodoCajas { get; set; }

        public DbSet<TipoTienda> TipoTiendas { get; set; }

        public DbSet<Articulo> Articulos { get; set; }

        public DbSet<CampañaTienda> CampañaTiendas { get; set; }

        public DbSet<CampañaArticulo> CampañaArticulos { get; set; }

        public DbSet<Proveedor> Proveedores { get; set; }

        public DbSet<Modulo> Modulos { get; set; }

        public DbSet<Operacion> Operaciones { get; set; }

        public DbSet<RolOperacion> RolOperaciones { get; set; }

        public DbSet<TiendaArticulo> TiendaArticulos { get; set; }

        public DbSet<CodigoCampaña> CodigosCampaña { get; set; }

        public DbSet<Familia> Familias { get; set; }

        public DbSet<TiendaConfiguracion> TiendaConfiguraciones { get; set; }

        public DbSet<TipoConfiguracion> TipoConfiguraciones { get; set; }

        public DbSet<AsignarConfiguracionTienda> AsignarConfiguracionTiendas { get; set; }

        public DbSet<Regla> Reglas { get; set; }

        public DbSet<TipoCampania> TipoCampanias { get; set; }

        public DbSet<ReglaMaterial> ReglaMateriales { get; set; }

        public DbSet<ReglaCatalogo> ReglasCatalogo { get; set; }

        public DbSet<ReglaCatalogoValor> ReglasCatalogoValor { get; set; }

        public DbSet<ReglaCaracteristica> ReglasCaracteristicas { get; set; }

        public DbSet<TiendaCaracteristica> TiendaCaracteristicas { get; set; }

        public DbSet<Bitacora> Bitacora { get; set; }

    }
}