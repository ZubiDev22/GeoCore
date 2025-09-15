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
            // BLD004 (Valencia) - Rentabilidad BAJA
            new CashFlow { CashFlowId = "CAF017", BuildingId = "BLD004", Date = DateTime.Parse("2019-07-22"), Amount = 120000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF018", BuildingId = "BLD004", Date = DateTime.Parse("2019-08-10"), Amount = 1000, Source = "Alquiler primer mes" },
            new CashFlow { CashFlowId = "CAF019", BuildingId = "BLD004", Date = DateTime.Parse("2019-09-10"), Amount = 950, Source = "Gasto comunidad" },
            // BLD006 (Granada) - Rentabilidad MEDIA
            new CashFlow { CashFlowId = "CAF020", BuildingId = "BLD006", Date = DateTime.Parse("2020-06-15"), Amount = 110000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF021", BuildingId = "BLD006", Date = DateTime.Parse("2020-07-10"), Amount = 4200, Source = "Alquiler primer mes" },
            new CashFlow { CashFlowId = "CAF022", BuildingId = "BLD006", Date = DateTime.Parse("2020-08-10"), Amount = 2000, Source = "Gasto comunidad" },
            // BLD010 (Estafeta, Pamplona)
            new CashFlow { CashFlowId = "CAF014", BuildingId = "BLD010", Date = DateTime.Parse("2020-03-18"), Amount = 420000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF015", BuildingId = "BLD010", Date = DateTime.Parse("2020-04-10"), Amount = 870, Source = "Alquiler primer mes" },
            new CashFlow { CashFlowId = "CAF016", BuildingId = "BLD010", Date = DateTime.Parse("2020-05-10"), Amount = 350, Source = "Gasto comunidad" }
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
