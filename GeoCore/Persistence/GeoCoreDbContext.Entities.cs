using GeoCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoCore.Persistence
{
    public partial class GeoCoreDbContext : DbContext
    {
        public DbSet<CashFlow> CashFlows { get; set; }
        public DbSet<MaintenanceEvent> MaintenanceEvents { get; set; }
        public DbSet<AssetAssessment> AssetAssessments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetAssessment>()
                .Property(a => a.Profitability)
                .HasPrecision(5, 2);

            modelBuilder.Entity<CashFlow>()
                .Property(c => c.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<MaintenanceEvent>()
                .Property(m => m.Cost)
                .HasPrecision(10, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
