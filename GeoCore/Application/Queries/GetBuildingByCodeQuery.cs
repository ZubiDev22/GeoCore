using MediatR;
using GeoCore.DTOs;

namespace GeoCore.Application.Queries
{
    public class GetBuildingByCodeQuery : IRequest<BuildingDto?>
    {
        public string Code { get; set; }
        public GetBuildingByCodeQuery(string code)
        {
            Code = code;
        }
    }
}