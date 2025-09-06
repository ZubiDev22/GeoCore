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
        public async Task<ActionResult<IEnumerable<MaintenanceEventDto>>> GetAll(
            [FromQuery] string? from = null,
            [FromQuery] string? to = null,
            [FromQuery] int? buildingId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _loguer.LogInfo($"Obteniendo eventos de mantenimiento. Filtros: from={from}, to={to}, buildingId={buildingId}, página={page}, tamaño={pageSize}");
            var events = await _repository.GetAllAsync();

            // Filtro por rango de fechas
            if (DateTime.TryParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDate))
                events = events.Where(e => e.Date >= fromDate);
            if (DateTime.TryParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDate))
                events = events.Where(e => e.Date <= toDate);

            // Filtro por edificio
            if (buildingId.HasValue)
                events = events.Where(e => e.BuildingId == buildingId.Value);

            var dtos = events
                .OrderByDescending(e => e.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new MaintenanceEventDto
                {
                    MaintenanceEventId = e.MaintenanceEventId,
                    MaintenanceEventCode = e.MaintenanceEventCode,
                    BuildingId = e.BuildingId,
                    Date = e.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Description = e.Description,
                    Cost = e.Cost
                });
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MaintenanceEventDto>> GetById(int id)
        {
            var e = await _repository.GetByIdAsync(id);
            if (e == null)
                return NotFound();
            var dto = new MaintenanceEventDto
            {
                MaintenanceEventId = e.MaintenanceEventId,
                MaintenanceEventCode = e.MaintenanceEventCode,
                BuildingId = e.BuildingId,
                Date = e.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Description = e.Description,
                Cost = e.Cost
            };
            return Ok(dto);
        }
    }
}
