using MediatR;
using GeoCore.DTOs;
using GeoCore.Entities;
using GeoCore.Repositories;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace GeoCore.Application.Handlers
{
    public class CreateBuildingCommandHandler : IRequestHandler<GeoCore.Application.Commands.CreateBuildingCommand, BuildingDto>
    {
        private readonly IBuildingRepository _repository;
        public CreateBuildingCommandHandler(IBuildingRepository repository)
        {
            _repository = repository;
        }
        public async Task<BuildingDto> Handle(GeoCore.Application.Commands.CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Building;
            var entity = new Building
            {
                BuildingCode = dto.BuildingCode,
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                PurchaseDate = DateTime.ParseExact(dto.PurchaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = dto.Status
            };
            await _repository.AddAsync(entity);
            // Asignar el BuildingId generado en el stub
            dto.BuildingId = entity.BuildingId;
            return dto;
        }
    }
}
