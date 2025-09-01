using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class CashFlowRepositoryStub : ICashFlowRepository
    {
        private readonly List<CashFlow> _cashFlows = new();
        public Task<IEnumerable<CashFlow>> GetAllAsync() => Task.FromResult(_cashFlows.AsEnumerable());
        public Task<CashFlow?> GetByIdAsync(int id) => Task.FromResult(_cashFlows.FirstOrDefault(c => c.Id == id));
        public Task AddAsync(CashFlow entity) { _cashFlows.Add(entity); return Task.CompletedTask; }
        public void Update(CashFlow entity) { }
        public void Remove(CashFlow entity) { _cashFlows.Remove(entity); }
    }
}
