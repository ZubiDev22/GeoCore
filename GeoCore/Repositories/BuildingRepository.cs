using GeoCore.Entities;
using GeoCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GeoCore.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly GeoCoreDbContext _context;
        public BuildingRepository(GeoCoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Building>> GetAllAsync()
        {
            return await _context.Buildings.ToListAsync();
        }

        public async Task<Building?> GetByIdAsync(string id)
        {
            return await _context.Buildings.FindAsync(id);
        }

        public async Task<Building?> GetByCodeAsync(string code)
        {
            return await _context.Buildings.FirstOrDefaultAsync(b => b.BuildingCode == code);
        }

        public async Task AddAsync(Building entity)
        {
            _context.Buildings.Add(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(Building entity)
        {
            _context.Buildings.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(Building entity)
        {
            _context.Buildings.Remove(entity);
            _context.SaveChanges();
        }
    }
}
