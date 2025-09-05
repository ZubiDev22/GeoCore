using MediatR;
using GeoCore.DTOs;
using System.Collections.Generic;

namespace GeoCore.Application.Queries
{
    public class GetApartmentsByBuildingCodeQuery : IRequest<IEnumerable<ApartmentDto>>
    {
        public string BuildingCode { get; set; }
        public GetApartmentsByBuildingCodeQuery(string buildingCode)
        {
            BuildingCode = buildingCode;
        }
    }
}
