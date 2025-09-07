using MediatR;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Application.Handlers
{
    public class CreateMaintenanceEventCommandHandler : IRequestHandler<GeoCore.Application.Commands.CreateMaintenanceEventCommand, MaintenanceEventDto>
    {
        private readonly IMaintenanceEventRepository _repository;
        public CreateMaintenanceEventCommandHandler(IMaintenanceEventRepository repository)
        {
            _repository = repository;
        }
        public async Task<MaintenanceEventDto> Handle(GeoCore.Application.Commands.CreateMaintenanceEventCommand request, CancellationToken cancellationToken)
        {
            var dto = request.MaintenanceEvent;
            var entity = new MaintenanceEvent
            {
                BuildingId = dto.BuildingId,
                Date = DateTime.ParseExact(dto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Description = dto.Description,
                Cost = dto.Cost
            };
            await _repository.AddAsync(entity);
            dto.MaintenanceEventId = entity.MaintenanceEventId;
            return dto;
        }
    }
}
