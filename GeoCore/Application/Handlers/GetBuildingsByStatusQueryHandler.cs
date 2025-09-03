using MediatR;
using GeoCore.DTOs;
using GeoCore.Repositories;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GeoCore.Application.Handlers
{
    public class GetBuildingsByStatusQueryHandler : IRequestHandler<GeoCore.Application.Queries.GetBuildingsByStatusQuery, IEnumerable<BuildingDto>>
    {
        private readonly IBuildingRepository _repository;
        public GetBuildingsByStatusQueryHandler(IBuildingRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<BuildingDto>> Handle(GeoCore.Application.Queries.GetBuildingsByStatusQuery request, CancellationToken cancellationToken)
        {
            var buildings = await _repository.GetAllAsync();
            var filtered = buildings.Where(b => b.Status.Equals(request.Status, System.StringComparison.OrdinalIgnoreCase));
            var dtos = filtered.Select(b => new BuildingDto
            {
                BuildingId = b.BuildingId,
                BuildingCode = b.BuildingCode,
                Name = b.Name,
                Address = b.Address,
                City = b.City,
                Latitude = b.Latitude,
                Longitude = b.Longitude,
                PurchaseDate = b.PurchaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = b.Status
            });
            return dtos;
        }
    }
}