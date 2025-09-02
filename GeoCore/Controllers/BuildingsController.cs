using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Linq;
using System.Globalization;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/buildings")]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingRepository _repository;

        public BuildingsController(IBuildingRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildingDto>>> GetAll(int page = 1, int pageSize = 10)
        {
            var buildings = await _repository.GetAllAsync();
            var paged = buildings.Skip((page - 1) * pageSize).Take(pageSize);
            var dtos = paged.Select(b => new BuildingDto
            {
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

        [HttpGet("{code}")]
        public async Task<ActionResult<BuildingDto>> GetByCode(string code)
        {
            var building = (await _repository.GetAllAsync()).FirstOrDefault(b => b.BuildingCode == code);
            if (building == null)
                return NotFound();
            var dto = new BuildingDto
            {
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
            var buildings = await _repository.GetAllAsync();
            var filtered = buildings.Where(b => b.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            var dtos = filtered.Select(b => new BuildingDto
            {
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BuildingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new Building
            {
                BuildingCode = dto.BuildingCode,
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                PurchaseDate = DateTime.ParseExact(dto.PurchaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = dto.Status
            };
            await _repository.AddAsync(entity);
            return Ok(dto);
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, [FromBody] BuildingDto dto)
        {
            var buildings = await _repository.GetAllAsync();
            var entity = buildings.FirstOrDefault(b => b.BuildingCode == code);
            if (entity == null)
                return NotFound();

            entity.Name = dto.Name;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.PurchaseDate = DateTime.ParseExact(dto.PurchaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            entity.Status = dto.Status;
            _repository.Update(entity);
            return Ok(dto);
        }

        [HttpPatch("{code}")]
        public async Task<IActionResult> Patch(string code, [FromBody] Dictionary<string, object> patch)
        {
            var buildings = await _repository.GetAllAsync();
            var entity = buildings.FirstOrDefault(b => b.BuildingCode == code);
            if (entity == null)
                return NotFound();

            foreach (var kvp in patch)
            {
                switch (kvp.Key.ToLower())
                {
                    case "name": entity.Name = kvp.Value?.ToString() ?? entity.Name; break;
                    case "address": entity.Address = kvp.Value?.ToString() ?? entity.Address; break;
                    case "city": entity.City = kvp.Value?.ToString() ?? entity.City; break;
                    case "latitude": if (double.TryParse(kvp.Value?.ToString(), out var lat)) entity.Latitude = lat; break;
                    case "longitude": if (double.TryParse(kvp.Value?.ToString(), out var lng)) entity.Longitude = lng; break;
                    case "purchasedate": if (DateTime.TryParseExact(kvp.Value?.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date)) entity.PurchaseDate = date; break;
                    case "status": entity.Status = kvp.Value?.ToString() ?? entity.Status; break;
                }
            }
            _repository.Update(entity);
            return Ok(entity);
        }

        [HttpPatch("{code}/status")]
        public async Task<IActionResult> PatchStatus(string code, [FromBody] string status)
        {
            var buildings = await _repository.GetAllAsync();
            var entity = buildings.FirstOrDefault(b => b.BuildingCode == code);
            if (entity == null)
                return NotFound();

            entity.Status = status;
            _repository.Update(entity);
            return Ok(new { BuildingCode = code, Status = status });
        }
    }
}
