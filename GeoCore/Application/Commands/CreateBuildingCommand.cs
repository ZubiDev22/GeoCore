using GeoCore.Application.Common;
using MediatR;
using GeoCore.DTOs;

namespace GeoCore.Application.Commands
{
    public class CreateBuildingCommand : IRequest<Result<BuildingDto>>
    {
        public BuildingDto Building { get; set; }
        public CreateBuildingCommand(BuildingDto building)
        {
            Building = building;
        }
    }
}
