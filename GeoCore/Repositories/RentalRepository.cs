using GeoCore.Entities;
using GeoCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GeoCore.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly GeoCoreDbContext _context;
        public RentalRepository(GeoCoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _context.Rentals.ToListAsync();
        }

        public async Task<Rental?> GetByIdAsync(string id)
        {
            return await _context.Rentals.FindAsync(id);
        }

        public async Task AddAsync(Rental entity)
        {
            _context.Rentals.Add(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(Rental entity)
        {
            _context.Rentals.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(Rental entity)
        {
            _context.Rentals.Remove(entity);
            _context.SaveChanges();
        }
    }
}
