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
        public async Task<ActionResult<IEnumerable<BuildingDto>>> GetAll()
        {
            var buildings = await _repository.GetAllAsync();
            var dtos = buildings.Select(b => new BuildingDto
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
    }
}
