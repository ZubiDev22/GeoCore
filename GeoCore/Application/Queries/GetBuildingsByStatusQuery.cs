using MediatR;
using GeoCore.DTOs;
using System.Collections.Generic;

namespace GeoCore.Application.Queries
{
    public class GetBuildingsByStatusQuery : IRequest<IEnumerable<BuildingDto>>
    {
        public string Status { get; set; }
        public GetBuildingsByStatusQuery(string status)
        {
            Status = status;
        }
    }
}