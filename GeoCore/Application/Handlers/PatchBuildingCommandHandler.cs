using MediatR;
using GeoCore.Entities;
using GeoCore.Repositories;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GeoCore.Application.Handlers
{
    public class PatchBuildingCommandHandler : IRequestHandler<GeoCore.Application.Commands.PatchBuildingCommand, bool>
    {
        private readonly IBuildingRepository _repository;
        public PatchBuildingCommandHandler(IBuildingRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(GeoCore.Application.Commands.PatchBuildingCommand request, CancellationToken cancellationToken)
        {
            var buildings = await _repository.GetAllAsync();
            var entity = buildings.FirstOrDefault(b => b.BuildingCode == request.Code);
            if (entity == null) return false;
            foreach (var kvp in request.Patch)
            {
                switch (kvp.Key.ToLower())
                {
                    case "name": entity.Name = kvp.Value?.ToString() ?? entity.Name; break;
                    case "address": entity.Address = kvp.Value?.ToString() ?? entity.Address; break;
                    case "city": entity.City = kvp.Value?.ToString() ?? entity.City; break;
                    case "latitude": if (double.TryParse(kvp.Value?.ToString(), out var lat)) entity.Latitude = lat; break;
                    case "longitude": if (double.TryParse(kvp.Value?.ToString(), out var lng)) entity.Longitude = lng; break;
                    case "purchasedate": if (DateTime.TryParseExact(kvp.Value?.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date)) entity.PurchaseDate = date; break;
                    case "status": entity.Status = kvp.Value?.ToString() ?? entity.Status; break;
                }
            }
            _repository.Update(entity);
            return true;
        }
    }
}