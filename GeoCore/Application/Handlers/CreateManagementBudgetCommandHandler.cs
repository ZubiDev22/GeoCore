using MediatR;
using GeoCore.DTOs;
using GeoCore.Entities;
using GeoCore.Repositories;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace GeoCore.Application.Handlers
{
    public class CreateManagementBudgetCommandHandler : IRequestHandler<GeoCore.Application.Commands.CreateManagementBudgetCommand, ManagementBudgetDto>
    {
        private readonly IManagementBudgetRepository _repository;
        public CreateManagementBudgetCommandHandler(IManagementBudgetRepository repository)
        {
            _repository = repository;
        }
        public async Task<ManagementBudgetDto> Handle(GeoCore.Application.Commands.CreateManagementBudgetCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ManagementBudget;
            var entity = new ManagementBudget
            {
                ManagementBudgetId = dto.ManagementBudgetId,
                BuildingCode = dto.BuildingCode,
                Date = DateTime.ParseExact(dto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Profitability = dto.Profitability,
                RiskLevel = dto.RiskLevel,
                Recommendation = dto.Recommendation
            };
            await _repository.AddAsync(entity);
            return dto;
        }
    }
}
