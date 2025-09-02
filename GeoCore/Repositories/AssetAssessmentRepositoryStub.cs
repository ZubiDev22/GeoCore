using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class ManagementBudgetRepositoryStub : IManagementBudgetRepository
    {
        private readonly List<ManagementBudget> _budgets = new()
        {
            new ManagementBudget { ManagementBudgetId = "MBG001", BuildingCode = "BLD001", Date = DateTime.Parse("01/07/2024"), Profitability = 8.5m, RiskLevel = "Low", Recommendation = "Mantener" },
            new ManagementBudget { ManagementBudgetId = "MBG002", BuildingCode = "BLD002", Date = DateTime.Parse("15/07/2024"), Profitability = 7.2m, RiskLevel = "Medium", Recommendation = "Revisar" },
            new ManagementBudget { ManagementBudgetId = "MBG003", BuildingCode = "BLD003", Date = DateTime.Parse("20/07/2024"), Profitability = 9.1m, RiskLevel = "Low", Recommendation = "Invertir" },
            new ManagementBudget { ManagementBudgetId = "MBG004", BuildingCode = "BLD004", Date = DateTime.Parse("05/08/2024"), Profitability = 6.8m, RiskLevel = "High", Recommendation = "Vender" },
            new ManagementBudget { ManagementBudgetId = "MBG005", BuildingCode = "BLD005", Date = DateTime.Parse("10/08/2024"), Profitability = 8.0m, RiskLevel = "Medium", Recommendation = "Mantener" },
            new ManagementBudget { ManagementBudgetId = "MBG006", BuildingCode = "BLD006", Date = DateTime.Parse("15/08/2024"), Profitability = 7.5m, RiskLevel = "Low", Recommendation = "Invertir" },
            new ManagementBudget { ManagementBudgetId = "MBG007", BuildingCode = "BLD007", Date = DateTime.Parse("20/08/2024"), Profitability = 9.0m, RiskLevel = "Low", Recommendation = "Mantener" },
            new ManagementBudget { ManagementBudgetId = "MBG008", BuildingCode = "BLD008", Date = DateTime.Parse("25/08/2024"), Profitability = 6.5m, RiskLevel = "High", Recommendation = "Vender" },
            new ManagementBudget { ManagementBudgetId = "MBG009", BuildingCode = "BLD009", Date = DateTime.Parse("30/08/2024"), Profitability = 8.2m, RiskLevel = "Medium", Recommendation = "Revisar" },
            new ManagementBudget { ManagementBudgetId = "MBG010", BuildingCode = "BLD010", Date = DateTime.Parse("04/09/2024"), Profitability = 7.8m, RiskLevel = "Low", Recommendation = "Mantener" }
        };
        public Task<IEnumerable<ManagementBudget>> GetAllAsync() => Task.FromResult(_budgets.AsEnumerable());
        public Task<ManagementBudget?> GetByIdAsync(string id) => Task.FromResult(_budgets.FirstOrDefault(b => b.ManagementBudgetId == id));
        public Task<ManagementBudget?> GetByIdAsync(int id)
        {
            // Busca por ManagementBudgetId con formato MBG{id:D3}
            return Task.FromResult(_budgets.FirstOrDefault(b => b.ManagementBudgetId == $"MBG{id:D3}"));
        }
        public Task AddAsync(ManagementBudget entity) { _budgets.Add(entity); return Task.CompletedTask; }
        public void Update(ManagementBudget entity)
        {
            var existing = _budgets.FirstOrDefault(b => b.ManagementBudgetId == entity.ManagementBudgetId);
            if (existing != null)
            {
                existing.BuildingCode = entity.BuildingCode;
                existing.Date = entity.Date;
                existing.Profitability = entity.Profitability;
                existing.RiskLevel = entity.RiskLevel;
                existing.Recommendation = entity.Recommendation;
            }
        }
        public void Remove(ManagementBudget entity) { _budgets.Remove(entity); }
    }
}
