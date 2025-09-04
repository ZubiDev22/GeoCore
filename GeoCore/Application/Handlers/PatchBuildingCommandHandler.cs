using MediatR;
using GeoCore.Entities;
using GeoCore.Repositories;
using GeoCore.DTOs;
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
        private readonly IMaintenanceEventRepository _maintenanceRepo;
        private readonly ICashFlowRepository _cashFlowRepo;
        public PatchBuildingCommandHandler(IBuildingRepository repository, IMaintenanceEventRepository maintenanceRepo, ICashFlowRepository cashFlowRepo)
        {
            _repository = repository;
            _maintenanceRepo = maintenanceRepo;
            _cashFlowRepo = cashFlowRepo;
        }
        public async Task<bool> Handle(GeoCore.Application.Commands.PatchBuildingCommand request, CancellationToken cancellationToken)
        {
            var buildings = await _repository.GetAllAsync();
            var entity = buildings.FirstOrDefault(b => b.BuildingCode == request.Code);
            if (entity == null) return false;
            foreach (var op in request.Operations)
            {
                switch (op.Path.ToLower())
                {
                    case "/status":
                        var newStatus = op.Value?.ToString() ?? entity.Status;
                        if (entity.Status == newStatus)
                        {
                            // No permitir cambiar al mismo status
                            return false;
                        }
                        entity.Status = newStatus;
                        // Si el status es "Under Maintenance", actualiza MaintenanceEvent
                        if (entity.Status == "Under Maintenance" && request.Operations.Any(o => o.Path == "/description"))
                        {
                            var descOp = request.Operations.First(o => o.Path == "/description");
                            var events = await _maintenanceRepo.GetAllAsync();
                            var lastEvent = events.Where(e => e.BuildingId == entity.BuildingId).OrderByDescending(e => e.Date).FirstOrDefault();
                            if (lastEvent != null)
                            {
                                lastEvent.Description = descOp.Value?.ToString() ?? lastEvent.Description;
                                _maintenanceRepo.Update(lastEvent);
                            }
                        }
                        // Si el status es "Active", actualiza CashFlow (si lo necesitas)
                        if (entity.Status == "Active" && request.Operations.Any(o => o.Path == "/description"))
                        {
                            var descOp = request.Operations.First(o => o.Path == "/description");
                            var cashflows = await _cashFlowRepo.GetAllAsync();
                            var lastFlow = cashflows.Where(c => c.BuildingId == entity.BuildingId).OrderByDescending(c => c.Date).FirstOrDefault();
                            if (lastFlow != null)
                            {
                                lastFlow.Source = descOp.Value?.ToString() ?? lastFlow.Source;
                                _cashFlowRepo.Update(lastFlow);
                            }
                        }
                        break;
                }
            }
            // Ejemplo LINQ: Filtrar edificios activos en Madrid y ordenarlos por nombre
            var edificiosActivosMadrid = (from b in buildings
                                          where b.Status == "Active" && b.City == "Madrid"
                                          orderby b.Name
                                          select b).ToList();

            // Ejemplo LINQ: Obtener nombres de edificios con longitud mayor a 6
            var nombresLargos = buildings.Where(b => b.Name.Length > 6).OrderBy(b => b.Name).Select(b => b.Name).ToList();

            // Ejemplo LINQ: Filtrar eventos de mantenimiento recientes para el edificio actual
            var eventosRecientes = (from e in await _maintenanceRepo.GetAllAsync()
                                    where e.BuildingId == entity.BuildingId && e.Date > DateTime.Now.AddMonths(-6)
                                    orderby e.Date descending
                                    select e).ToList();

            _repository.Update(entity);
            return true;
        }
    }
}