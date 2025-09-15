using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class MaintenanceEventRepositoryStub : IMaintenanceEventRepository
    {
        private readonly List<MaintenanceEvent> _maintenanceEvents = new()
        {
            new MaintenanceEvent { MaintenanceEventId = "MAE001", BuildingId = "BLD001", Description = "Reparación ascensor", Cost = 1200, Date = DateTime.Parse("2023-01-15") },
            new MaintenanceEvent { MaintenanceEventId = "MAE002", BuildingId = "BLD002", Description = "Pintura fachada", Cost = 800, Date = DateTime.Parse("2023-02-10") },
            new MaintenanceEvent { MaintenanceEventId = "MAE003", BuildingId = "BLD003", Description = "Cambio caldera", Cost = 1500, Date = DateTime.Parse("2023-03-05") },
            new MaintenanceEvent { MaintenanceEventId = "MAE004", BuildingId = "BLD011", Description = "Reparación tejado ático", Cost = 1200, Date = DateTime.Parse("2023-05-15") },
            new MaintenanceEvent { MaintenanceEventId = "MAE005", BuildingId = "BLD010", Description = "Reparación portal", Cost = 900, Date = DateTime.Parse("2023-06-10") },
            new MaintenanceEvent { MaintenanceEventId = "MAE006", BuildingId = "BLD004", Description = "Reparación ascensor", Cost = 800, Date = DateTime.Parse("2023-07-10") },
            new MaintenanceEvent { MaintenanceEventId = "MAE007", BuildingId = "BLD006", Description = "Pintura fachada", Cost = 1200, Date = DateTime.Parse("2023-08-10") },
            new MaintenanceEvent { MaintenanceEventId = "MAE008", BuildingId = "BLD005", Description = "Revisión eléctrica", Cost = 600, Date = DateTime.Parse("2023-09-01") },
            new MaintenanceEvent { MaintenanceEventId = "MAE009", BuildingId = "BLD007", Description = "Limpieza garaje", Cost = 300, Date = DateTime.Parse("2023-09-15") },
            new MaintenanceEvent { MaintenanceEventId = "MAE010", BuildingId = "BLD008", Description = "Cambio portero automático", Cost = 450, Date = DateTime.Parse("2023-10-05") },
            new MaintenanceEvent { MaintenanceEventId = "MAE011", BuildingId = "BLD009", Description = "Impermeabilización azotea", Cost = 1100, Date = DateTime.Parse("2023-10-20") },
        };

        public Task<IEnumerable<MaintenanceEvent>> GetAllAsync() => Task.FromResult(_maintenanceEvents.AsEnumerable());
        public Task<MaintenanceEvent?> GetByIdAsync(string id) => Task.FromResult(_maintenanceEvents.FirstOrDefault(e => e.MaintenanceEventId == id));
        public Task AddAsync(MaintenanceEvent entity)
        {
            int nextNum = 1;
            if (_maintenanceEvents.Any())
            {
                var last = _maintenanceEvents
                    .Select(e => e.MaintenanceEventId)
                    .Where(id => id.StartsWith("MAE"))
                    .Select(id => int.TryParse(id.Substring(3), out var n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max();
                nextNum = last + 1;
            }
            entity.MaintenanceEventId = $"MAE{nextNum.ToString("D3")}";
            _maintenanceEvents.Add(entity);
            return Task.CompletedTask;
        }
        public void Update(MaintenanceEvent entity) { }
        public void Remove(MaintenanceEvent entity) { _maintenanceEvents.Remove(entity); }
    }
}
