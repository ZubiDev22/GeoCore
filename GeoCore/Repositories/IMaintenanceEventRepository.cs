using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public interface IMaintenanceEventRepository
    {
        Task<IEnumerable<MaintenanceEvent>> GetAllAsync();
        Task<MaintenanceEvent?> GetByIdAsync(string id);
        Task AddAsync(MaintenanceEvent entity);
        void Update(MaintenanceEvent entity);
        void Remove(MaintenanceEvent entity);
    }
}
