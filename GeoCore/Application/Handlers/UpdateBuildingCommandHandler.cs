using MediatR;
using GeoCore.DTOs;
using GeoCore.Entities;
using GeoCore.Repositories;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeoCore.Application.Handlers
{
    public class UpdateBuildingCommandHandler : IRequestHandler<GeoCore.Application.Commands.UpdateBuildingCommand, BuildingDto?>
    {
        private readonly IBuildingRepository _repository;
        public UpdateBuildingCommandHandler(IBuildingRepository repository)
        {
            _repository = repository;
        }
        public async Task<BuildingDto?> Handle(GeoCore.Application.Commands.UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
            var buildings = await _repository.GetAllAsync();
            var entity = buildings.FirstOrDefault(b => b.BuildingCode == request.Code);
            if (entity == null) return null;
            var dto = request.Building;
            entity.Name = dto.Name;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.PurchaseDate = DateTime.ParseExact(dto.PurchaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            entity.Status = dto.Status;
            _repository.Update(entity);
            return dto;
        }
    }
}