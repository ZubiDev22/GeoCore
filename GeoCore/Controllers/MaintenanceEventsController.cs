using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using GeoCore.Logging;
using MediatR;
using GeoCore.Application.Commands;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/maintenance-events")]
    public class MaintenanceEventsController : ControllerBase
    {
        private readonly IMaintenanceEventRepository _repository;
        private readonly ILoguer _loguer;
        private readonly IMediator _mediator;
        public MaintenanceEventsController(IMaintenanceEventRepository repository, ILoguer loguer, IMediator mediator)
        {
            _repository = repository;
            _loguer = loguer;
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceEventDto>>> GetAll(int page = 1, int pageSize = 10) {
            _loguer.LogInfo($"Obteniendo eventos de mantenimiento: página {page}, tamaño {pageSize}");
            var result = await _repository.GetAllAsync();
            var paged = result.Skip((page - 1) * pageSize).Take(pageSize);
            var dtos = paged.Select(e => new MaintenanceEventDto {
                MaintenanceEventId = e.MaintenanceEventId,
                BuildingCode = e.BuildingCode,
                Date = e.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Description = e.Description,
                Cost = e.Cost
            });
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MaintenanceEventDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateMaintenanceEventCommand(dto));
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] List<PatchOperation> operations)
        {
            if (operations == null || !operations.Any())
            {
                return BadRequest("No operations provided");
            }

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            var dto = new MaintenanceEventDto
            {
                MaintenanceEventId = entity.MaintenanceEventId,
                BuildingCode = entity.BuildingCode,
                Date = entity.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Description = entity.Description,
                Cost = entity.Cost
            };

            foreach (var operation in operations)
            {
                try
                {
                    ApplyOperation(dto, operation);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest($"Error in operation: {ex.Message}");
                }
            }

            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(dto, validationContext, validationResults, true))
            {
                return BadRequest(validationResults);
            }

            entity.BuildingCode = dto.BuildingCode;
            if (DateTime.TryParseExact(dto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                entity.Date = parsedDate;
            entity.Description = dto.Description;
            entity.Cost = dto.Cost;
            _repository.Update(entity);

            return Ok(dto);
        }

        private void ApplyOperation(MaintenanceEventDto dto, PatchOperation operation)
        {
            switch (operation.Op.ToLower())
            {
                case "replace":
                    ApplyReplaceOperation(dto, operation.Path, operation.Value);
                    break;
                case "add":
                    throw new ArgumentException("Add operation not supported yet");
                case "remove":
                    throw new ArgumentException("Remove operation not supported yet");
                default:
                    throw new ArgumentException($"Unsupported operation: {operation.Op}");
            }
        }

        private void ApplyReplaceOperation(MaintenanceEventDto dto, string path, object? value)
        {
            switch (path.ToLower())
            {
                case "/description":
                    dto.Description = value?.ToString() ?? throw new ArgumentException("Description cannot be null");
                    break;
                case "/cost":
                    if (value is decimal decimalValue)
                        dto.Cost = decimalValue;
                    else if (decimal.TryParse(value?.ToString(), out decimal parsedCost))
                        dto.Cost = parsedCost;
                    else
                        throw new ArgumentException("Invalid value for cost");
                    break;
                case "/maintenanceeventid":
                    dto.MaintenanceEventId = value?.ToString() ?? throw new ArgumentException("MaintenanceEventId cannot be null");
                    break;
                case "/buildingcode":
                    dto.BuildingCode = value?.ToString() ?? throw new ArgumentException("BuildingCode cannot be null");
                    break;
                case "/date":
                    dto.Date = value?.ToString() ?? throw new ArgumentException("Date cannot be null");
                    break;
                default:
                    throw new ArgumentException($"Unsupported path: {path}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            _repository.Remove(entity);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceEventDto>> GetById(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            var dto = new MaintenanceEventDto
            {
                MaintenanceEventId = entity.MaintenanceEventId,
                BuildingCode = entity.BuildingCode,
                Date = entity.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Description = entity.Description,
                Cost = entity.Cost
            };
            return Ok(dto);
        }
    }
}
