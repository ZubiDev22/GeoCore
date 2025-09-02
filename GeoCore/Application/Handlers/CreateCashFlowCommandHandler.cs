using MediatR;
using GeoCore.DTOs;
using GeoCore.Entities;
using GeoCore.Repositories;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace GeoCore.Application.Handlers
{
    public class CreateCashFlowCommandHandler : IRequestHandler<GeoCore.Application.Commands.CreateCashFlowCommand, CashFlowDto>
    {
        private readonly ICashFlowRepository _repository;
        public CreateCashFlowCommandHandler(ICashFlowRepository repository)
        {
            _repository = repository;
        }
        public async Task<CashFlowDto> Handle(GeoCore.Application.Commands.CreateCashFlowCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CashFlow;
            var entity = new CashFlow
            {
                CashFlowId = dto.CashFlowId,
                BuildingCode = dto.BuildingCode,
                Date = DateTime.ParseExact(dto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Amount = dto.Amount,
                Source = dto.Source
            };
            await _repository.AddAsync(entity);
            return dto;
        }
    }
}
