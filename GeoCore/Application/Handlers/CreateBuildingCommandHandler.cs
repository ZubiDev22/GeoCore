using GeoCore.Application.Common;
using MediatR;
using GeoCore.DTOs;
using GeoCore.Entities;
using GeoCore.Repositories;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace GeoCore.Application.Handlers
{
    public class CreateBuildingCommandHandler : IRequestHandler<GeoCore.Application.Commands.CreateBuildingCommand, Result<BuildingDto>>
    {
        private readonly IBuildingRepository _repository;
        public CreateBuildingCommandHandler(IBuildingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<BuildingDto>> Handle(GeoCore.Application.Commands.CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Building;
            // Validación simple de ejemplo
            if (string.IsNullOrWhiteSpace(dto.BuildingCode))
                return Result<BuildingDto>.Failure(new ValidationError("BuildingCode is required."));
            try
            {
                var entity = new Building
                {
                    BuildingCode = dto.BuildingCode,
                    Name = dto.Name,
                    Address = dto.Address,
                    City = dto.City,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    PurchaseDate = dto.PurchaseDate,
                    Status = dto.Status
                };
                await _repository.AddAsync(entity);
                dto.BuildingId = entity.BuildingId;
                return Result<BuildingDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<BuildingDto>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}"));
            }
        }
    }
}
