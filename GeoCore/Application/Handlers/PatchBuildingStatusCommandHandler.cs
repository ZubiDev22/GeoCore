using MediatR;
using GeoCore.Entities;
using GeoCore.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeoCore.Application.Handlers
{
    public class PatchBuildingStatusCommandHandler : IRequestHandler<GeoCore.Application.Commands.PatchBuildingStatusCommand, bool>
    {
        private readonly IBuildingRepository _repository;
        public PatchBuildingStatusCommandHandler(IBuildingRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(GeoCore.Application.Commands.PatchBuildingStatusCommand request, CancellationToken cancellationToken)
        {
            var buildings = await _repository.GetAllAsync();
            var entity = buildings.FirstOrDefault(b => b.BuildingCode == request.Code);
            if (entity == null) return false;
            entity.Status = request.Status;
            _repository.Update(entity);
            return true;
        }
    }
}