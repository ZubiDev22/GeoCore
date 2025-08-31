using GeoCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoCore.Persistence
{
    public partial class GeoCoreDbContext : DbContext
    {
        public DbSet<CashFlow> CashFlows { get; set; }
        public DbSet<MaintenanceEvent> MaintenanceEvents { get; set; }
        public DbSet<AssetAssessment> AssetAssessments { get; set; }
    }
}
