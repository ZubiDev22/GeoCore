using MediatR;
using GeoCore.DTOs;
using GeoCore.Repositories;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GeoCore.Application.Handlers
{
    public class GetBuildingsQueryHandler : IRequestHandler<GeoCore.Application.Queries.GetBuildingsQuery, IEnumerable<BuildingDto>>
    {
        private readonly IBuildingRepository _repository;
        public GetBuildingsQueryHandler(IBuildingRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<BuildingDto>> Handle(GeoCore.Application.Queries.GetBuildingsQuery request, CancellationToken cancellationToken)
        {
            var buildings = await _repository.GetAllAsync();
            var paged = buildings.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            var dtos = paged.Select(b => new BuildingDto
            {
                BuildingCode = b.BuildingCode,
                Name = b.Name,
                Address = b.Address,
                City = b.City,
                Latitude = b.Latitude,
                Longitude = b.Longitude,
                PurchaseDate = b.PurchaseDate,
                Status = b.Status
            });
            return dtos;
        }
    }
}