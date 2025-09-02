using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/cashflows")]
    public class CashFlowsController : ControllerBase
    {
        private readonly ICashFlowRepository _repository;
        public CashFlowsController(ICashFlowRepository repository) { _repository = repository; }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CashFlowDto>>> GetAll() {
            var result = await _repository.GetAllAsync();
            var dtos = result.Select(c => new CashFlowDto {
                CashFlowId = c.CashFlowId,
                BuildingCode = c.BuildingCode,
                Date = c.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Amount = c.Amount,
                Source = c.Source
            });
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CashFlowDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = new CashFlow
            {
                CashFlowId = dto.CashFlowId,
                BuildingCode = dto.BuildingCode,
                Date = DateTime.ParseExact(dto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Amount = dto.Amount,
                Source = dto.Source
            };
            await _repository.AddAsync(entity);
            return Ok(dto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] List<PatchOperation> operations)
        {
            if (operations == null || !operations.Any())
            {
                return BadRequest("No operations provided");
            }

            // Buscar la entidad correctamente
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            // Mapear a DTO para trabajar con él
            var dto = new CashFlowDto
            {
                CashFlowId = entity.CashFlowId,
                BuildingCode = entity.BuildingCode,
                Date = entity.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Amount = entity.Amount,
                Source = entity.Source
            };

            // Aplicar cada operación
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

            // Validar el DTO después de los cambios
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(dto, validationContext, validationResults, true))
            {
                return BadRequest(validationResults);
            }

            // Actualizar el stub realmente
            entity.BuildingCode = dto.BuildingCode;
            if (DateTime.TryParseExact(dto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                entity.Date = parsedDate;
            entity.Amount = dto.Amount;
            entity.Source = dto.Source;
            _repository.Update(entity);

            return Ok(dto);
        }

        private void ApplyOperation(CashFlowDto dto, PatchOperation operation)
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

        private void ApplyReplaceOperation(CashFlowDto dto, string path, object? value)
        {
            switch (path.ToLower())
            {
                case "/amount":
                    if (value is decimal decimalValue)
                        dto.Amount = decimalValue;
                    else if (decimal.TryParse(value?.ToString(), out decimal parsedAmount))
                        dto.Amount = parsedAmount;
                    else
                        throw new ArgumentException("Invalid value for amount");
                    break;
                case "/source":
                    dto.Source = value?.ToString() ?? throw new ArgumentException("Source cannot be null");
                    break;
                case "/description":
                    // No existe en CashFlowDto, ignorar
                    break;
                case "/cashflowid":
                    dto.CashFlowId = value?.ToString() ?? throw new ArgumentException("CashFlowId cannot be null");
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
        public async Task<ActionResult<CashFlowDto>> GetById(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            var dto = new CashFlowDto
            {
                CashFlowId = entity.CashFlowId,
                BuildingCode = entity.BuildingCode,
                Date = entity.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Amount = entity.Amount,
                Source = entity.Source
            };
            return Ok(dto);
        }
    }
}
