using MediatR;
using GeoCore.DTOs;

namespace GeoCore.Application.Commands
{
    public class UpdateBuildingCommand : IRequest<BuildingDto?>
    {
        public string Code { get; set; }
        public BuildingDto Building { get; set; }
        public UpdateBuildingCommand(string code, BuildingDto building)
        {
            Code = code;
            Building = building;
        }
    }
}