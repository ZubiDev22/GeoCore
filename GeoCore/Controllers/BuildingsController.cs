using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Linq;
using System.Globalization;
using GeoCore.Logging;
using MediatR;
using GeoCore.Application.Commands;
using GeoCore.Application.Queries;
using System.Collections.Generic;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/buildings")]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingRepository _repository;
        private readonly ILoguer _loguer;
        private readonly IMediator _mediator;

        public BuildingsController(IBuildingRepository repository, ILoguer loguer, IMediator mediator)
        {
            _repository = repository;
            _loguer = loguer;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildingDto>>> GetAll(int page = 1, int pageSize = 10)
        {
            _loguer.LogInfo($"Obteniendo edificios: página {page}, tamaño {pageSize}");
            var buildings = await _repository.GetAllAsync();
            var dtos = buildings.Skip((page - 1) * pageSize).Take(pageSize).Select(b => new BuildingDto
            {
                BuildingId = b.BuildingId,
                BuildingCode = b.BuildingCode,
                Name = b.Name,
                Address = b.Address,
                City = b.City,
                Latitude = b.Latitude,
                Longitude = b.Longitude,
                PurchaseDate = b.PurchaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = b.Status
            });
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BuildingDto>> GetById(int id)
        {
            var building = await _repository.GetByIdAsync(id);
            if (building == null)
                return NotFound();
            var dto = new BuildingDto
            {
                BuildingId = building.BuildingId,
                BuildingCode = building.BuildingCode,
                Name = building.Name,
                Address = building.Address,
                City = building.City,
                Latitude = building.Latitude,
                Longitude = building.Longitude,
                PurchaseDate = building.PurchaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = building.Status
            };
            return Ok(dto);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<BuildingDto>> GetByCode(string code)
        {
            var building = await _repository.GetByCodeAsync(code);
            if (building == null)
                return NotFound();
            var dto = new BuildingDto
            {
                BuildingId = building.BuildingId,
                BuildingCode = building.BuildingCode,
                Name = building.Name,
                Address = building.Address,
                City = building.City,
                Latitude = building.Latitude,
                Longitude = building.Longitude,
                PurchaseDate = building.PurchaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = building.Status
            };
            return Ok(dto);
        }

        [HttpGet("code/{code}/details")]
        public async Task<ActionResult<object>> GetDetailsByCode(string code)
        {
            var building = await _repository.GetByCodeAsync(code);
            if (building == null)
                return NotFound();
            // Obtener status
            var status = building.Status;
            // Obtener descripción del último MaintenanceEvent si el status es Under Maintenance
            string? description = null;
            if (status == "Under Maintenance")
            {
                var maintenanceRepo = HttpContext.RequestServices.GetService<IMaintenanceEventRepository>();
                if (maintenanceRepo != null)
                {
                    var events = await maintenanceRepo.GetAllAsync();
                    var lastEvent = events.Where(e => e.BuildingId == building.BuildingId).OrderByDescending(e => e.Date).FirstOrDefault();
                    description = lastEvent?.Description;
                }
            }
            // Obtener descripción del último CashFlow si el status es Active
            if (status == "Active")
            {
                var cashFlowRepo = HttpContext.RequestServices.GetService<ICashFlowRepository>();
                if (cashFlowRepo != null)
                {
                    var cashflows = await cashFlowRepo.GetAllAsync();
                    var lastFlow = cashflows.Where(c => c.BuildingId == building.BuildingId).OrderByDescending(c => c.Date).FirstOrDefault();
                    description = lastFlow?.Source;
                }
            }
            return Ok(new {
                BuildingId = building.BuildingId,
                BuildingCode = building.BuildingCode,
                Name = building.Name,
                Address = building.Address,
                City = building.City,
                Latitude = building.Latitude,
                Longitude = building.Longitude,
                PurchaseDate = building.PurchaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = status,
                Description = description
            });
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<BuildingDto>>> GetByStatus(string status)
        {
            var dtos = await _mediator.Send(new GetBuildingsByStatusQuery(status));
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BuildingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateBuildingCommand(dto));
            return Ok(result);
        }

        // [HttpPut("{code}")]
        // public async Task<IActionResult> Update(string code, [FromBody] BuildingDto dto)
        // {
        //     var result = await _mediator.Send(new UpdateBuildingCommand(code, dto));
        //     if (result == null)
        //         return NotFound();
        //     return Ok(result);
        // }

        [HttpPatch("{code}")]
        public async Task<IActionResult> Patch(string code, [FromBody] List<PatchOperation> operations)
        {
            var success = await _mediator.Send(new PatchBuildingCommand(code, operations));
            if (!success)
                return NotFound();
            return Ok();
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var success = await _mediator.Send(new DeleteBuildingCommand(code));
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
