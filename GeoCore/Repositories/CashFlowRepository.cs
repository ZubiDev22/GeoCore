using GeoCore.Entities;
using GeoCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GeoCore.Repositories
{
    public class CashFlowRepository : ICashFlowRepository
    {
        private readonly GeoCoreDbContext _context;
        public CashFlowRepository(GeoCoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CashFlow>> GetAllAsync()
        {
            return await _context.CashFlows.ToListAsync();
        }

        public async Task<CashFlow?> GetByIdAsync(string id)
        {
            return await _context.CashFlows.FindAsync(id);
        }

        public async Task AddAsync(CashFlow entity)
        {
            _context.CashFlows.Add(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(CashFlow entity)
        {
            _context.CashFlows.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(CashFlow entity)
        {
            _context.CashFlows.Remove(entity);
            _context.SaveChanges();
        }
    }
}
