using GeoCore.Application.Common;
using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/maintenance-events")]
    public class MaintenanceEventsController : ControllerBase
    {
        private readonly IMaintenanceEventRepository _repository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly ILogger<MaintenanceEventsController> _logger;

        public MaintenanceEventsController(IMaintenanceEventRepository repository, IBuildingRepository buildingRepository, ILogger<MaintenanceEventsController> logger)
        {
            _repository = repository;
            _buildingRepository = buildingRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<MaintenanceEventDto>>> GetAll(
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] string? buildingId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation($"[MaintenanceEventsController] Obteniendo eventos de mantenimiento. Filtros: from={from}, to={to}, buildingId={buildingId}, página={page}, tamaño={pageSize}");
                var events = await _repository.GetAllAsync();
                if (from.HasValue)
                    events = events.Where(e => e.Date >= from.Value);
                if (to.HasValue)
                    events = events.Where(e => e.Date <= to.Value);
                if (!string.IsNullOrEmpty(buildingId))
                    events = events.Where(e => e.BuildingId == buildingId);
                var totalItems = events.Count();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var dtos = events
                    .OrderByDescending(e => e.Date)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new MaintenanceEventDto
                    {
                        MaintenanceEventId = e.MaintenanceEventId,
                        BuildingId = e.BuildingId,
                        Date = e.Date,
                        Description = e.Description,
                        Cost = e.Cost
                    }).ToList();
                return Ok(new PagedResultDto<MaintenanceEventDto> { Items = dtos, TotalPages = totalPages });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[MaintenanceEventsController] Error inesperado al obtener eventos de mantenimiento");
                return StatusCode(500, Result<IEnumerable<MaintenanceEventDto>>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<MaintenanceEventDto>>> GetById(string id)
        {
            try
            {
                var e = await _repository.GetByIdAsync(id);
                if (e == null)
                    return NotFound(Result<MaintenanceEventDto>.Failure(new NotFoundError($"MaintenanceEvent with id '{id}' not found.")));
                var dto = new MaintenanceEventDto
                {
                    MaintenanceEventId = e.MaintenanceEventId,
                    BuildingId = e.BuildingId,
                    Date = e.Date, // DateTime para formato ISO 8601
                    Description = e.Description,
                    Cost = e.Cost
                };
                return Ok(Result<MaintenanceEventDto>.Success(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[MaintenanceEventsController] Error inesperado al obtener evento de mantenimiento {id}");
                return StatusCode(500, Result<MaintenanceEventDto>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }
    }
}
