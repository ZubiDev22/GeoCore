using MediatR;
using GeoCore.DTOs;
using System.Collections.Generic;

namespace GeoCore.Application.Queries
{
    public class GetBuildingsQuery : IRequest<IEnumerable<BuildingDto>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public GetBuildingsQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}