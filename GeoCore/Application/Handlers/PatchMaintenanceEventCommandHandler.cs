using MediatR;
using GeoCore.Repositories;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace GeoCore.Application.Handlers
{
    public class PatchMaintenanceEventCommandHandler : IRequestHandler<GeoCore.Application.Commands.PatchMaintenanceEventCommand, bool>
    {
        private readonly IMaintenanceEventRepository _repository;
        public PatchMaintenanceEventCommandHandler(IMaintenanceEventRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(GeoCore.Application.Commands.PatchMaintenanceEventCommand request, CancellationToken cancellationToken)
        {
            var events = await _repository.GetAllAsync();
            var entity = events.FirstOrDefault(e => e.MaintenanceEventId == request.MaintenanceEventId);
            if (entity == null) return false;
            foreach (var kvp in request.Patch)
            {
                switch (kvp.Key.ToLower())
                {
                    case "description": entity.Description = kvp.Value?.ToString() ?? entity.Description; break;
                    case "date": if (DateTime.TryParseExact(kvp.Value?.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date)) entity.Date = date; break;
                    case "cost": if (decimal.TryParse(kvp.Value?.ToString(), out var cost)) entity.Cost = cost; break;
                }
            }
            _repository.Update(entity);
            return true;
        }
    }
}
