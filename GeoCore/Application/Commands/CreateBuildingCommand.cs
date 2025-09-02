using MediatR;
using GeoCore.DTOs;

namespace GeoCore.Application.Commands
{
    public class CreateBuildingCommand : IRequest<BuildingDto>
    {
        public BuildingDto Building { get; set; }
        public CreateBuildingCommand(BuildingDto building)
        {
            Building = building;
        }
    }
}
