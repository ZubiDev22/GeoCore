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

        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, [FromBody] BuildingDto dto)
        {
            var result = await _mediator.Send(new UpdateBuildingCommand(code, dto));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPatch("{code}")]
        public async Task<IActionResult> Patch(string code, [FromBody] Dictionary<string, object> patch)
        {
            var success = await _mediator.Send(new PatchBuildingCommand(code, patch));
            if (!success)
                return NotFound();
            return Ok();
        }

        [HttpPatch("{code}/status")]
        public async Task<IActionResult> PatchStatus(string code, [FromBody] string status)
        {
            var success = await _mediator.Send(new PatchBuildingStatusCommand(code, status));
            if (!success)
                return NotFound();
            return Ok(new { BuildingCode = code, Status = status });
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
