using GeoCore.Entities;
using System.Threading.Tasks;

namespace GeoCore.Repositories
{
    public interface IMaintenanceEventRepository : IGenericRepository<MaintenanceEvent>
    {
        Task<MaintenanceEvent?> GetByIdAsync(string id);
    }
}
