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
            new ManagementBudget { ManagementBudgetId = 1, ManagementBudgetCode = "MB001", BuildingId = 2, Date = DateTime.Parse("2023-03-01"), Profitability = 0.08m, RiskLevel = "Bajo", Recommendation = "Alquiler recomendado por rentabilidad" },
            new ManagementBudget { ManagementBudgetId = 2, ManagementBudgetCode = "MB002", BuildingId = 5, Date = DateTime.Parse("2023-04-10"), Profitability = 0.06m, RiskLevel = "Medio", Recommendation = "Alquiler aceptable, revisar condiciones" }
        };
        public Task<IEnumerable<ManagementBudget>> GetAllAsync() => Task.FromResult(_budgets.AsEnumerable());
        public Task<ManagementBudget?> GetByIdAsync(int id) => Task.FromResult(_budgets.FirstOrDefault(b => b.ManagementBudgetId == id));
        public Task AddAsync(ManagementBudget entity) { _budgets.Add(entity); return Task.CompletedTask; }
        public void Update(ManagementBudget entity) { }
        public void Remove(ManagementBudget entity) { _budgets.Remove(entity); }
    }
}
