using GeoCore.Entities;
using GeoCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GeoCore.Repositories
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly GeoCoreDbContext _context;
        public ApartmentRepository(GeoCoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Apartment>> GetAllAsync()
        {
            return await _context.Apartments.ToListAsync();
        }

        public async Task<Apartment?> GetByIdAsync(string id)
        {
            return await _context.Apartments.FindAsync(id);
        }

        public async Task<IEnumerable<Apartment>> GetByBuildingIdAsync(string buildingId)
        {
            return await _context.Apartments.Where(a => a.BuildingId == buildingId).ToListAsync();
        }

        public async Task AddAsync(Apartment entity)
        {
            _context.Apartments.Add(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(Apartment entity)
        {
            _context.Apartments.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(Apartment entity)
        {
            _context.Apartments.Remove(entity);
            _context.SaveChanges();
        }
    }
}
