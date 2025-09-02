using GeoCore.Entities;
using System.Threading.Tasks;

namespace GeoCore.Repositories
{
    public interface IManagementBudgetRepository : IGenericRepository<ManagementBudget>
    {
        Task<ManagementBudget?> GetByIdAsync(string id);
    }
}
