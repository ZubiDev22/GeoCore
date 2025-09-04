using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class CashFlowRepositoryStub : ICashFlowRepository
    {
        private readonly List<CashFlow> _cashFlows = new()
        {
            new CashFlow { CashFlowId = 1, CashFlowCode = "CF001", BuildingId = 1, Date = DateTime.Parse("2023-01-15"), Amount = 10000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = 2, CashFlowCode = "CF002", BuildingId = 2, Date = DateTime.Parse("2023-02-10"), Amount = 8000, Source = "Alquiler primer mes" }
        };
        public Task<IEnumerable<CashFlow>> GetAllAsync() => Task.FromResult(_cashFlows.AsEnumerable());
        public Task<CashFlow?> GetByIdAsync(int id) => Task.FromResult(_cashFlows.FirstOrDefault(c => c.CashFlowId == id));
        public Task AddAsync(CashFlow entity) { _cashFlows.Add(entity); return Task.CompletedTask; }
        public void Update(CashFlow entity) { }
        public void Remove(CashFlow entity) { _cashFlows.Remove(entity); }
    }
}
