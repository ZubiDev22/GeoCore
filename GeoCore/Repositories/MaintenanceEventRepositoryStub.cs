using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class MaintenanceEventRepositoryStub : IMaintenanceEventRepository
    {
        private readonly List<MaintenanceEvent> _events = new();
        public Task<IEnumerable<MaintenanceEvent>> GetAllAsync() => Task.FromResult(_events.AsEnumerable());
        public Task<MaintenanceEvent?> GetByIdAsync(int id) => Task.FromResult(_events.FirstOrDefault(e => e.Id == id));
        public Task AddAsync(MaintenanceEvent entity) { _events.Add(entity); return Task.CompletedTask; }
        public void Update(MaintenanceEvent entity) { }
        public void Remove(MaintenanceEvent entity) { _events.Remove(entity); }
    }
}
