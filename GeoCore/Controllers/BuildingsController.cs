using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Entities;
using GeoCore.Repositories;

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
            // Implementación simulada
            var buildings = await _repository.GetAllAsync();
            var dtos = buildings.Select(b => new BuildingDto
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                City = b.City,
                Latitude = b.Latitude,
                Longitude = b.Longitude,
                PurchaseDate = b.PurchaseDate,
                Status = b.Status
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuildingDto>> GetById(int id)
        {
            var building = await _repository.GetByIdAsync(id);
            if (building == null)
                return NotFound();
            var dto = new BuildingDto
            {
                Id = building.Id,
                Name = building.Name,
                Address = building.Address,
                City = building.City,
                Latitude = building.Latitude,
                Longitude = building.Longitude,
                PurchaseDate = building.PurchaseDate,
                Status = building.Status
            };
            return Ok(dto);
        }
    }
}
