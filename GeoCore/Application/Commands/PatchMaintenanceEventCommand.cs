using MediatR;
using System.Collections.Generic;

namespace GeoCore.Application.Commands
{
    public class PatchMaintenanceEventCommand : IRequest<bool>
    {
        public int MaintenanceEventId { get; set; }
        public Dictionary<string, object> Patch { get; set; }
        public PatchMaintenanceEventCommand(int maintenanceEventId, Dictionary<string, object> patch)
        {
            MaintenanceEventId = maintenanceEventId;
            Patch = patch;
        }
    }
}
