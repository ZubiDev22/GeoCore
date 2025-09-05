using MediatR;
using GeoCore.DTOs;
using GeoCore.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GeoCore.Application.Handlers
{
    public class GetApartmentsByBuildingCodeQueryHandler : IRequestHandler<GeoCore.Application.Queries.GetApartmentsByBuildingCodeQuery, IEnumerable<ApartmentDto>>
    {
        private readonly IApartmentRepository _apartmentRepo;
        private readonly IBuildingRepository _buildingRepo;
        public GetApartmentsByBuildingCodeQueryHandler(IApartmentRepository apartmentRepo, IBuildingRepository buildingRepo)
        {
            _apartmentRepo = apartmentRepo;
            _buildingRepo = buildingRepo;
        }
        public async Task<IEnumerable<ApartmentDto>> Handle(GeoCore.Application.Queries.GetApartmentsByBuildingCodeQuery request, CancellationToken cancellationToken)
        {
            var buildings = await _buildingRepo.GetAllAsync();
            var building = buildings.FirstOrDefault(b => b.BuildingCode == request.BuildingCode);
            if (building == null) return Enumerable.Empty<ApartmentDto>();
            var apartments = await _apartmentRepo.GetByBuildingIdAsync(building.BuildingId);
            return apartments.Select(a => new ApartmentDto
            {
                ApartmentId = a.ApartmentId,
                ApartmentCode = a.ApartmentCode,
                ApartmentDoor = a.ApartmentDoor,
                ApartmentFloor = a.ApartmentFloor,
                ApartmentPrice = a.ApartmentPrice,
                NumberOfRooms = a.NumberOfRooms,
                NumberOfBathrooms = a.NumberOfBathrooms,
                BuildingId = a.BuildingId,
                HasLift = a.HasLift,
                HasGarage = a.HasGarage,
                CreatedDate = a.CreatedDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            });
        }
    }
}
