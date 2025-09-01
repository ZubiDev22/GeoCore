// Carpeta para persistencia (placeholder para DbContext)
using Microsoft.EntityFrameworkCore;
using GeoCore.Entities;

namespace GeoCore.Persistence
{
    public partial class GeoCoreDbContext : DbContext
    {
        public GeoCoreDbContext(DbContextOptions<GeoCoreDbContext> options) : base(options) { }

        public DbSet<Building> Buildings { get; set; }
        // Agregar DbSet para otras entidades
    }
}
