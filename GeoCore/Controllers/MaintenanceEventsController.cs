using GeoCore.Application.Common;
using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Linq;
using System.Globalization;
using GeoCore.Logging;
using System.Collections.Generic;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/maintenance-events")]
    public class MaintenanceEventsController : ControllerBase
    {
        private readonly IMaintenanceEventRepository _repository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly ILoguer _loguer;

        public MaintenanceEventsController(IMaintenanceEventRepository repository, IBuildingRepository buildingRepository, ILoguer loguer)
        {
            _repository = repository;
            _buildingRepository = buildingRepository;
            _loguer = loguer;
        }

        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<MaintenanceEventDto>>>> GetAll(
            [FromQuery] string? from = null,
            [FromQuery] string? to = null,
            [FromQuery] string? buildingId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                _loguer.LogInfo($"Obteniendo eventos de mantenimiento. Filtros: from={from}, to={to}, buildingId={buildingId}, página={page}, tamaño={pageSize}");
                var events = await _repository.GetAllAsync();
                if (DateTime.TryParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDate))
                    events = events.Where(e => e.Date >= fromDate);
                if (DateTime.TryParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDate))
                    events = events.Where(e => e.Date <= toDate);
                if (buildingId != null)
                    events = events.Where(e => e.BuildingId == buildingId.ToString());
                var dtos = events
                    .OrderByDescending(e => e.Date)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new MaintenanceEventDto
                    {
                        MaintenanceEventId = e.MaintenanceEventId,
                        BuildingId = e.BuildingId,
                        Date = e.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Description = e.Description,
                        Cost = e.Cost
                    });
                return Ok(Result<IEnumerable<MaintenanceEventDto>>.Success(dtos));
            }
            catch (Exception ex)
            {
                _loguer.LogError("Error inesperado al obtener eventos de mantenimiento", ex);
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
                    Date = e.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Description = e.Description,
                    Cost = e.Cost
                };
                return Ok(Result<MaintenanceEventDto>.Success(dto));
            }
            catch (Exception ex)
            {
                _loguer.LogError($"Error inesperado al obtener evento de mantenimiento {id}", ex);
                return StatusCode(500, Result<MaintenanceEventDto>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }
    }
}
