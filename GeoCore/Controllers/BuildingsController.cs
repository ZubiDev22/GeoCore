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
            var dtos = await _mediator.Send(new GetBuildingsQuery(page, pageSize));
            return Ok(dtos);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<BuildingDto>> GetByCode(string code)
        {
            var dto = await _mediator.Send(new GetBuildingByCodeQuery(code));
            if (dto == null)
                return NotFound();
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
