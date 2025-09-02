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
            new CashFlow { CashFlowId = "GST001", BuildingCode = "BLD001", Date = DateTime.Parse("01/07/2024"), Amount = 1200.50m, Source = "Alquiler" },
            new CashFlow { CashFlowId = "GST002", BuildingCode = "BLD002", Date = DateTime.Parse("15/07/2024"), Amount = 950.00m, Source = "Venta" },
            new CashFlow { CashFlowId = "GST003", BuildingCode = "BLD003", Date = DateTime.Parse("20/07/2024"), Amount = 500.00m, Source = "Mantenimiento" },
            new CashFlow { CashFlowId = "GST004", BuildingCode = "BLD004", Date = DateTime.Parse("05/08/2024"), Amount = 1800.00m, Source = "Alquiler" },
            new CashFlow { CashFlowId = "GST005", BuildingCode = "BLD005", Date = DateTime.Parse("10/08/2024"), Amount = 750.00m, Source = "Venta" },
            new CashFlow { CashFlowId = "GST006", BuildingCode = "BLD006", Date = DateTime.Parse("15/08/2024"), Amount = 600.00m, Source = "Mantenimiento" },
            new CashFlow { CashFlowId = "GST007", BuildingCode = "BLD007", Date = DateTime.Parse("20/08/2024"), Amount = 2100.00m, Source = "Alquiler" },
            new CashFlow { CashFlowId = "GST008", BuildingCode = "BLD008", Date = DateTime.Parse("25/08/2024"), Amount = 850.00m, Source = "Venta" },
            new CashFlow { CashFlowId = "GST009", BuildingCode = "BLD009", Date = DateTime.Parse("30/08/2024"), Amount = 400.00m, Source = "Mantenimiento" },
            new CashFlow { CashFlowId = "GST010", BuildingCode = "BLD010", Date = DateTime.Parse("04/09/2024"), Amount = 1300.00m, Source = "Alquiler" }
        };
        public Task<IEnumerable<CashFlow>> GetAllAsync() => Task.FromResult(_cashFlows.AsEnumerable());
        public Task<CashFlow?> GetByIdAsync(string id) => Task.FromResult(_cashFlows.FirstOrDefault(c => c.CashFlowId == id));
        public Task<CashFlow?> GetByIdAsync(int id)
        {
            // Busca por CashFlowId con formato GST{id:D3}
            return Task.FromResult(_cashFlows.FirstOrDefault(c => c.CashFlowId == $"GST{id:D3}"));
        }
        public Task AddAsync(CashFlow entity) { _cashFlows.Add(entity); return Task.CompletedTask; }
        public void Update(CashFlow entity)
        {
            var existing = _cashFlows.FirstOrDefault(c => c.CashFlowId == entity.CashFlowId);
            if (existing != null)
            {
                existing.BuildingCode = entity.BuildingCode;
                existing.Date = entity.Date;
                existing.Amount = entity.Amount;
                existing.Source = entity.Source;
            }
        }
        public void Remove(CashFlow entity) { _cashFlows.Remove(entity); }
    }
}
