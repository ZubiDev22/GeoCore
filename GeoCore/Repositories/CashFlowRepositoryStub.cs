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
            // BLD001 (Madrid) - Rentabilidad positiva
            new CashFlow { CashFlowId = "CAF001", BuildingId = "BLD001", Date = DateTime.Parse("2020-01-10"), Amount = 350000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF002", BuildingId = "BLD001", Date = DateTime.Parse("2021-01-10"), Amount = 20000, Source = "Alquiler anual" },
            // BLD002 (Barcelona) - Rentabilidad positiva
            new CashFlow { CashFlowId = "CAF003", BuildingId = "BLD002", Date = DateTime.Parse("2021-05-20"), Amount = 320000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF004", BuildingId = "BLD002", Date = DateTime.Parse("2022-01-10"), Amount = 18000, Source = "Alquiler anual" },
            // BLD003 (Sevilla) - Rentabilidad positiva
            new CashFlow { CashFlowId = "CAF005", BuildingId = "BLD003", Date = DateTime.Parse("2022-03-15"), Amount = 280000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF006", BuildingId = "BLD003", Date = DateTime.Parse("2023-01-10"), Amount = 17000, Source = "Alquiler anual" },
            // BLD004 (Valencia) - Rentabilidad positiva
            new CashFlow { CashFlowId = "CAF007", BuildingId = "BLD004", Date = DateTime.Parse("2019-07-22"), Amount = 250000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF008", BuildingId = "BLD004", Date = DateTime.Parse("2020-01-10"), Amount = 16000, Source = "Alquiler anual" },
            // BLD005 (Bilbao) - Rentabilidad positiva
            new CashFlow { CashFlowId = "CAF009", BuildingId = "BLD005", Date = DateTime.Parse("2018-11-30"), Amount = 400000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF010", BuildingId = "BLD005", Date = DateTime.Parse("2019-01-10"), Amount = 19000, Source = "Alquiler anual" },
            // BLD006 (Granada) - Rentabilidad positiva
            new CashFlow { CashFlowId = "CAF011", BuildingId = "BLD006", Date = DateTime.Parse("2020-06-15"), Amount = 210000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF012", BuildingId = "BLD006", Date = DateTime.Parse("2021-01-10"), Amount = 14000, Source = "Alquiler anual" },
            // BLD010 (Pamplona, Estafeta) - Rentabilidad positiva
            new CashFlow { CashFlowId = "CAF013", BuildingId = "BLD010", Date = DateTime.Parse("2020-03-18"), Amount = 300000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF014", BuildingId = "BLD010", Date = DateTime.Parse("2021-01-10"), Amount = 24000, Source = "Alquiler anual" },
            // BLD011 (Pamplona, Pérez Goyena) - Rentabilidad positiva
            new CashFlow { CashFlowId = "CAF015", BuildingId = "BLD011", Date = DateTime.Parse("2021-09-01"), Amount = 270000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF016", BuildingId = "BLD011", Date = DateTime.Parse("2022-01-10"), Amount = 22000, Source = "Alquiler anual" },
            // Otros edificios (sin rentabilidad garantizada)
            new CashFlow { CashFlowId = "CAF017", BuildingId = "BLD007", Date = DateTime.Parse("2021-02-20"), Amount = 90000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF018", BuildingId = "BLD008", Date = DateTime.Parse("2019-09-10"), Amount = 95000, Source = "Compra inicial" },
            new CashFlow { CashFlowId = "CAF019", BuildingId = "BLD009", Date = DateTime.Parse("2022-01-05"), Amount = 100000, Source = "Compra inicial" },
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
