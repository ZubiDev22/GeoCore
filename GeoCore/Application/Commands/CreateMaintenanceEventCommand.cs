using MediatR;
using GeoCore.DTOs;

namespace GeoCore.Application.Commands
{
    public class CreateMaintenanceEventCommand : IRequest<MaintenanceEventDto>
    {
        public MaintenanceEventDto MaintenanceEvent { get; set; }
        public CreateMaintenanceEventCommand(MaintenanceEventDto maintenanceEvent)
        {
            MaintenanceEvent = maintenanceEvent;
        }
    }
}
