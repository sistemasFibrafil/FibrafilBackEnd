using Net.Business.Entities.Sap;
using Net.Business.Entities.Web;
using Microsoft.EntityFrameworkCore;

namespace Net.Business.Services.Models
{
    public class DataContext : DbContext
    {
        /// <summary>
        /// GENERAL
        /// </summary>
        public DbSet<SerieSapEntity> SerieSap { get; set; }
        

        /// <summary>
        /// ARTICULOS
        /// </summary>
        public DbSet<GrupoArticuloSapEntity> GrupoArticuloSap { get; set; }
        public DbSet<SubGrupoArticuloSapEntity> SubGrupoArticuloSap { get; set; }
        public DbSet<SubGrupoArticulo2SapEntity> SubGrupoArticulo2Sap { get; set; }
        public DbSet<SedeSapEntity> SedeSap { get; set; }



        /// <summary>
        /// SOCIO DE NEGOCIOS
        /// </summary>
        public DbSet<GrupoSocioNegocioSapEntity> GrupoSocioNegocioSap { get; set; }
        public DbSet<SectorSocioNegocioSapEntity> SectorSocioNegocioSap { get; set; }



        /// <summary>
        /// VENTAS
        /// </summary>
        public DbSet<ForcastVentaConSinOcEntity> ForcastVentaConSinOc { get; set; }
        public DbSet<ForcastVentaNegocioEntity> ForcastVentaNegocio { get; set; }
        public DbSet<ForcastVentaEstadoEntity> ForcastVentaEstado { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
