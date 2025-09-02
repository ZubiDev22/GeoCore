using MediatR;
using GeoCore.Entities;
using GeoCore.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeoCore.Application.Handlers
{
    public class DeleteBuildingCommandHandler : IRequestHandler<GeoCore.Application.Commands.DeleteBuildingCommand, bool>
    {
        private readonly IBuildingRepository _repository;
        public DeleteBuildingCommandHandler(IBuildingRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(GeoCore.Application.Commands.DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
            var buildings = await _repository.GetAllAsync();
            var entity = buildings.FirstOrDefault(b => b.BuildingCode == request.Code);
            if (entity == null) return false;
            _repository.Remove(entity);
            return true;
        }
    }
}