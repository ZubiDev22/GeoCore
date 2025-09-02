using MediatR;
using GeoCore.DTOs;
using GeoCore.Repositories;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeoCore.Application.Handlers
{
    public class GetBuildingByCodeQueryHandler : IRequestHandler<GeoCore.Application.Queries.GetBuildingByCodeQuery, BuildingDto?>
    {
        private readonly IBuildingRepository _repository;
        public GetBuildingByCodeQueryHandler(IBuildingRepository repository)
        {
            _repository = repository;
        }
        public async Task<BuildingDto?> Handle(GeoCore.Application.Queries.GetBuildingByCodeQuery request, CancellationToken cancellationToken)
        {
            var building = (await _repository.GetAllAsync()).FirstOrDefault(b => b.BuildingCode == request.Code);
            if (building == null) return null;
            return new BuildingDto
            {
                BuildingCode = building.BuildingCode,
                Name = building.Name,
                Address = building.Address,
                City = building.City,
                Latitude = building.Latitude,
                Longitude = building.Longitude,
                PurchaseDate = building.PurchaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = building.Status
            };
        }
    }
}