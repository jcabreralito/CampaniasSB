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

        public DbSet<Esquema> Esquemas { get; set; }

        public DbSet<Articulo> Articulos { get; set; }

        public DbSet<CampañaArticulo> CampañaArticulos { get; set; }

        public DbSet<Modulo> Modulos { get; set; }

        public DbSet<Operacion> Operaciones { get; set; }

        public DbSet<RolOperacion> RolOperaciones { get; set; }

        public DbSet<TiendaArticulo> TiendaArticulos { get; set; }

        public DbSet<CodigoCampaña> CodigosCampaña { get; set; }

        public DbSet<Regla> Reglas { get; set; }

        public DbSet<ReglaCatalogo> ReglasCatalogo { get; set; }

        public DbSet<ReglaCaracteristica> ReglasCaracteristicas { get; set; }

        public DbSet<TiendaCaracteristica> TiendaCaracteristicas { get; set; }

        public DbSet<Bitacora> Bitacora { get; set; }

    }
}