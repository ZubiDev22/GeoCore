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
            new CashFlow { CashFlowId = "CAF001", BuildingId = "BLD001", Date = DateTime.Parse("2020-01-10"), Amount = 10000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF002", BuildingId = "BLD002", Date = DateTime.Parse("2021-05-20"), Amount = 8000, Source = "Alquiler primer mes" },
            new CashFlow { CashFlowId = "CAF003", BuildingId = "BLD003", Date = DateTime.Parse("2022-03-15"), Amount = 9500, Source = "Reforma tras compra" },
            new CashFlow { CashFlowId = "CAF004", BuildingId = "BLD004", Date = DateTime.Parse("2019-07-25"), Amount = 12000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF005", BuildingId = "BLD005", Date = DateTime.Parse("2018-12-05"), Amount = 7000, Source = "Alquiler primer mes" },
            new CashFlow { CashFlowId = "CAF006", BuildingId = "BLD006", Date = DateTime.Parse("2020-06-20"), Amount = 11000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF007", BuildingId = "BLD007", Date = DateTime.Parse("2021-02-25"), Amount = 9000, Source = "Reforma tras compra" },
            new CashFlow { CashFlowId = "CAF008", BuildingId = "BLD008", Date = DateTime.Parse("2019-09-15"), Amount = 10500, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF009", BuildingId = "BLD009", Date = DateTime.Parse("2022-01-10"), Amount = 8000, Source = "Alquiler primer mes" },
            new CashFlow { CashFlowId = "CAF010", BuildingId = "BLD010", Date = DateTime.Parse("2020-03-25"), Amount = 11500, Source = "Compra inicial" }
        };
        public Task<IEnumerable<CashFlow>> GetAllAsync() => Task.FromResult(_cashFlows.AsEnumerable());
        public Task<CashFlow?> GetByIdAsync(string id) => Task.FromResult(_cashFlows.FirstOrDefault(c => c.CashFlowId == id));
        public Task AddAsync(CashFlow entity)
        {
            int nextNum = 1;
            if (_cashFlows.Any())
            {
                var last = _cashFlows
                    .Select(c => c.CashFlowId)
                    .Where(id => id.StartsWith("CAF"))
                    .Select(id => int.TryParse(id.Substring(3), out var n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max();
                nextNum = last + 1;
            }
            entity.CashFlowId = $"CAF{nextNum.ToString("D3")}";
            _cashFlows.Add(entity);
            return Task.CompletedTask;
        }
        public void Update(CashFlow entity) { }
        public void Remove(CashFlow entity) { _cashFlows.Remove(entity); }
    }
}
