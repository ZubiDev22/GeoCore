using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/management-budgets")]
    public class ManagementBudgetsController : ControllerBase
    {
        private readonly IManagementBudgetRepository _repository;
        public ManagementBudgetsController(IManagementBudgetRepository repository) { _repository = repository; }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManagementBudgetDto>>> GetAll() {
            var result = await _repository.GetAllAsync();
            var dtos = result.Select(b => new ManagementBudgetDto {
                ManagementBudgetId = b.ManagementBudgetId,
                BuildingCode = b.BuildingCode,
                Date = b.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Profitability = b.Profitability,
                RiskLevel = b.RiskLevel,
                Recommendation = b.Recommendation
            });
            return Ok(dtos);
        }
        [HttpPost]
        public IActionResult Create([FromBody] ManagementBudgetDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = new ManagementBudget {
                ManagementBudgetId = dto.ManagementBudgetId,
                BuildingCode = dto.BuildingCode,
                Date = DateTime.ParseExact(dto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Profitability = dto.Profitability,
                RiskLevel = dto.RiskLevel,
                Recommendation = dto.Recommendation
            };
            _repository.AddAsync(entity);
            return Ok(dto);
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

            var dto = new ManagementBudgetDto
            {
                ManagementBudgetId = entity.ManagementBudgetId ?? string.Empty,
                BuildingCode = entity.BuildingCode ?? string.Empty,
                Date = entity.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Profitability = entity.Profitability,
                RiskLevel = entity.RiskLevel ?? string.Empty,
                Recommendation = entity.Recommendation ?? string.Empty
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
            entity.Profitability = dto.Profitability;
            entity.RiskLevel = dto.RiskLevel;
            entity.Recommendation = dto.Recommendation;
            _repository.Update(entity);

            return Ok(dto);
        }

        private void ApplyOperation(ManagementBudgetDto dto, PatchOperation operation)
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

        private void ApplyReplaceOperation(ManagementBudgetDto dto, string path, object? value)
        {
            switch (path.ToLower())
            {
                case "/profitability":
                    if (value is decimal decimalValue)
                        dto.Profitability = decimalValue;
                    else if (decimal.TryParse(value?.ToString(), out decimal parsedProfit))
                        dto.Profitability = parsedProfit;
                    else
                        throw new ArgumentException("Invalid value for profitability");
                    break;
                case "/risklevel":
                    dto.RiskLevel = value?.ToString() ?? throw new ArgumentException("RiskLevel cannot be null");
                    break;
                case "/recommendation":
                    dto.Recommendation = value?.ToString();
                    break;
                case "/managementbudgetid":
                    dto.ManagementBudgetId = value?.ToString() ?? throw new ArgumentException("ManagementBudgetId cannot be null");
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
        public IActionResult Delete(string id)
        {
            // Simulación: eliminar el ManagementBudget por id
            // Aquí deberías eliminar el real
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ManagementBudgetDto>> GetById(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            var dto = new ManagementBudgetDto
            {
                ManagementBudgetId = entity.ManagementBudgetId ?? string.Empty,
                BuildingCode = entity.BuildingCode ?? string.Empty,
                Date = entity.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Profitability = entity.Profitability,
                RiskLevel = entity.RiskLevel ?? string.Empty,
                Recommendation = entity.Recommendation ?? string.Empty
            };
            return Ok(dto);
        }
    }
}
