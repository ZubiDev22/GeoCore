using GeoCore.Entities;
using GeoCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GeoCore.Repositories
{
    public class MaintenanceEventRepository : IMaintenanceEventRepository
    {
        private readonly GeoCoreDbContext _context;
        public MaintenanceEventRepository(GeoCoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaintenanceEvent>> GetAllAsync()
        {
            return await _context.MaintenanceEvents.ToListAsync();
        }

        public async Task<MaintenanceEvent?> GetByIdAsync(string id)
        {
            return await _context.MaintenanceEvents.FindAsync(id);
        }

        public async Task AddAsync(MaintenanceEvent entity)
        {
            _context.MaintenanceEvents.Add(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(MaintenanceEvent entity)
        {
            _context.MaintenanceEvents.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(MaintenanceEvent entity)
        {
            _context.MaintenanceEvents.Remove(entity);
            _context.SaveChanges();
        }
    }
}
