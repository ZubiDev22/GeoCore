using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public interface IManagementBudgetRepository
    {
        Task<IEnumerable<ManagementBudget>> GetAllAsync();
        Task<ManagementBudget?> GetByIdAsync(int id);
        Task AddAsync(ManagementBudget entity);
        void Update(ManagementBudget entity);
        void Remove(ManagementBudget entity);
    }
}
