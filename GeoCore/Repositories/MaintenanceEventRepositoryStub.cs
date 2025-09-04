using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class MaintenanceEventRepositoryStub : IMaintenanceEventRepository
    {
        private readonly List<MaintenanceEvent> _events = new()
        {
            new MaintenanceEvent { MaintenanceEventId = 1, MaintenanceEventCode = "MNE001", BuildingId = 1, Date = DateTime.Parse("2023-01-10"), Description = "Mantenimiento inicial", Cost = 500 },
            new MaintenanceEvent { MaintenanceEventId = 2, MaintenanceEventCode = "MNE002", BuildingId = 3, Date = DateTime.Parse("2023-02-15"), Description = "Reparación de ascensor", Cost = 1200 },
            new MaintenanceEvent { MaintenanceEventId = 3, MaintenanceEventCode = "MNE003", BuildingId = 7, Date = DateTime.Parse("2023-03-20"), Description = "Pintura exterior", Cost = 800 }
        };
        public Task<IEnumerable<MaintenanceEvent>> GetAllAsync() => Task.FromResult(_events.AsEnumerable());
        public Task<MaintenanceEvent?> GetByIdAsync(int id) => Task.FromResult(_events.FirstOrDefault(e => e.MaintenanceEventId == id));
        public Task AddAsync(MaintenanceEvent entity) { _events.Add(entity); return Task.CompletedTask; }
        public void Update(MaintenanceEvent entity) { }
        public void Remove(MaintenanceEvent entity) { _events.Remove(entity); }
    }
}
