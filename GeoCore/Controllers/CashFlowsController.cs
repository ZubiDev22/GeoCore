using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Linq;
using System.Globalization;
using GeoCore.Logging;
using MediatR;
using System.Collections.Generic;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/cashflows")]
    public class CashFlowsController : ControllerBase
    {
        private readonly ICashFlowRepository _repository;
        private readonly ILoguer _loguer;
        private readonly IMediator _mediator;

        public CashFlowsController(ICashFlowRepository repository, ILoguer loguer, IMediator mediator)
        {
            _repository = repository;
            _loguer = loguer;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CashFlowDto>>> GetAll(
            [FromQuery] string? from = null,
            [FromQuery] string? to = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool orderByDateDesc = true,
            [FromQuery] decimal? minAmount = null,
            [FromQuery] decimal? maxAmount = null)
        {
            _loguer.LogInfo($"Obteniendo cashflows. Página: {page}, tamaño: {pageSize}, orden descendente: {orderByDateDesc}, from: {from}, to: {to}, minAmount: {minAmount}, maxAmount: {maxAmount}");
            var cashflows = await _repository.GetAllAsync();
            var buildings = await new BuildingRepositoryStub().GetAllAsync();

            // Filtro por rango de fechas
            if (DateTime.TryParseExact(from, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var fromDate))
                cashflows = cashflows.Where(c => c.Date >= fromDate);
            if (DateTime.TryParseExact(to, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var toDate))
                cashflows = cashflows.Where(c => c.Date <= toDate);

            // Filtro por importe
            if (minAmount.HasValue)
                cashflows = cashflows.Where(c => c.Amount >= minAmount.Value);
            if (maxAmount.HasValue)
                cashflows = cashflows.Where(c => c.Amount <= maxAmount.Value);

            // Ordenación por fecha
            cashflows = orderByDateDesc
                ? cashflows.OrderByDescending(c => c.Date)
                : cashflows.OrderBy(c => c.Date);

            var dtos = cashflows
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CashFlowDto
                {
                    CashFlowId = c.CashFlowId,
                    BuildingId = c.BuildingId,
                    BuildingCode = buildings.FirstOrDefault(b => b.BuildingId == c.BuildingId)?.BuildingCode ?? string.Empty,
                    Date = c.Date.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    Amount = c.Amount,
                    Source = c.Source
                });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CashFlowDto>> GetById(string id)
        {
            var cashflow = await _repository.GetByIdAsync(id);
            if (cashflow == null)
                return NotFound();
            var dto = new CashFlowDto
            {
                CashFlowId = cashflow.CashFlowId,
                BuildingId = cashflow.BuildingId,
                Date = cashflow.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Amount = cashflow.Amount,
                Source = cashflow.Source
            };
            return Ok(dto);
        }

        [HttpGet("/api/buildings/{code}/cashflows")]
        public async Task<ActionResult<IEnumerable<CashFlowDto>>> GetByBuildingCode(string code, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _loguer.LogInfo($"Obteniendo cashflows para el edificio {code}, página: {page}, tamaño: {pageSize}");
            var buildings = await new BuildingRepositoryStub().GetAllAsync();
            var building = buildings.FirstOrDefault(b => b.BuildingCode == code);
            if (building == null)
                return NotFound();
            var cashflows = await _repository.GetAllAsync();
            var filtered = cashflows.Where(c => c.BuildingId == building.BuildingId);
            var dtos = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CashFlowDto
                {
                    CashFlowId = c.CashFlowId,
                    BuildingId = c.BuildingId,
                    BuildingCode = code,
                    Date = c.Date.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    Amount = c.Amount,
                    Source = c.Source
                });
            return Ok(dtos);
        }
    }
}
