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
            new MaintenanceEvent { MaintenanceEventId = "MNT001", BuildingCode = "BLD001", Date = DateTime.Parse("05/07/2024"), Description = "Revisión anual de ascensor", Cost = 350.00m },
            new MaintenanceEvent { MaintenanceEventId = "MNT002", BuildingCode = "BLD002", Date = DateTime.Parse("10/07/2024"), Description = "Pintura de fachada", Cost = 1200.00m },
            new MaintenanceEvent { MaintenanceEventId = "MNT003", BuildingCode = "BLD003", Date = DateTime.Parse("15/07/2024"), Description = "Reparación de tejado", Cost = 800.00m },
            new MaintenanceEvent { MaintenanceEventId = "MNT004", BuildingCode = "BLD004", Date = DateTime.Parse("20/07/2024"), Description = "Cambio de caldera", Cost = 950.00m },
            new MaintenanceEvent { MaintenanceEventId = "MNT005", BuildingCode = "BLD005", Date = DateTime.Parse("25/07/2024"), Description = "Limpieza de garaje", Cost = 400.00m },
            new MaintenanceEvent { MaintenanceEventId = "MNT006", BuildingCode = "BLD006", Date = DateTime.Parse("30/07/2024"), Description = "Reparación de ventanas", Cost = 600.00m },
            new MaintenanceEvent { MaintenanceEventId = "MNT007", BuildingCode = "BLD007", Date = DateTime.Parse("04/08/2024"), Description = "Revisión eléctrica", Cost = 300.00m },
            new MaintenanceEvent { MaintenanceEventId = "MNT008", BuildingCode = "BLD008", Date = DateTime.Parse("09/08/2024"), Description = "Pintura interior", Cost = 700.00m },
            new MaintenanceEvent { MaintenanceEventId = "MNT009", BuildingCode = "BLD009", Date = DateTime.Parse("14/08/2024"), Description = "Reparación de ascensor", Cost = 1200.00m },
            new MaintenanceEvent { MaintenanceEventId = "MNT010", BuildingCode = "BLD010", Date = DateTime.Parse("19/08/2024"), Description = "Limpieza de fachada", Cost = 500.00m }
        };
        public Task<IEnumerable<MaintenanceEvent>> GetAllAsync() => Task.FromResult(_events.AsEnumerable());
        public Task<MaintenanceEvent?> GetByIdAsync(int id) => Task.FromResult(_events.FirstOrDefault(e => e.MaintenanceEventId == $"MNT{id:D3}"));
        public Task<MaintenanceEvent?> GetByIdAsync(string id) => Task.FromResult(_events.FirstOrDefault(e => e.MaintenanceEventId == id));
        public Task AddAsync(MaintenanceEvent entity) { _events.Add(entity); return Task.CompletedTask; }
        public void Update(MaintenanceEvent entity)
        {
            var existing = _events.FirstOrDefault(e => e.MaintenanceEventId == entity.MaintenanceEventId);
            if (existing != null)
            {
                existing.BuildingCode = entity.BuildingCode;
                existing.Date = entity.Date;
                existing.Description = entity.Description;
                existing.Cost = entity.Cost;
            }
        }
        public void Remove(MaintenanceEvent entity) { _events.Remove(entity); }
    }
}
