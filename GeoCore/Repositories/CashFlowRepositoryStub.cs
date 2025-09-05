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
            new CashFlow { CashFlowId = 1, CashFlowCode = "CF001", BuildingId = 1, Date = DateTime.Parse("2020-01-10"), Amount = 10000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = 2, CashFlowCode = "CF002", BuildingId = 2, Date = DateTime.Parse("2021-05-20"), Amount = 8000, Source = "Alquiler primer mes" },
            new CashFlow { CashFlowId = 3, CashFlowCode = "CF003", BuildingId = 3, Date = DateTime.Parse("2022-03-15"), Amount = 9500, Source = "Reforma tras compra" },
            new CashFlow { CashFlowId = 4, CashFlowCode = "CF004", BuildingId = 4, Date = DateTime.Parse("2019-07-25"), Amount = 12000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = 5, CashFlowCode = "CF005", BuildingId = 5, Date = DateTime.Parse("2018-12-05"), Amount = 7000, Source = "Alquiler primer mes" },
            new CashFlow { CashFlowId = 6, CashFlowCode = "CF006", BuildingId = 6, Date = DateTime.Parse("2020-06-20"), Amount = 11000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = 7, CashFlowCode = "CF007", BuildingId = 7, Date = DateTime.Parse("2021-02-25"), Amount = 9000, Source = "Reforma tras compra" },
            new CashFlow { CashFlowId = 8, CashFlowCode = "CF008", BuildingId = 8, Date = DateTime.Parse("2019-09-15"), Amount = 10500, Source = "Compra inicial" },
            new CashFlow { CashFlowId = 9, CashFlowCode = "CF009", BuildingId = 9, Date = DateTime.Parse("2022-01-10"), Amount = 8000, Source = "Alquiler primer mes" },
            new CashFlow { CashFlowId = 10, CashFlowCode = "CF010", BuildingId = 10, Date = DateTime.Parse("2020-03-25"), Amount = 11500, Source = "Compra inicial" }
        };
        public Task<IEnumerable<CashFlow>> GetAllAsync() => Task.FromResult(_cashFlows.AsEnumerable());
        public Task<CashFlow?> GetByIdAsync(int id) => Task.FromResult(_cashFlows.FirstOrDefault(c => c.CashFlowId == id));
        public Task AddAsync(CashFlow entity) { _cashFlows.Add(entity); return Task.CompletedTask; }
        public void Update(CashFlow entity) { }
        public void Remove(CashFlow entity) { _cashFlows.Remove(entity); }
    }
}
