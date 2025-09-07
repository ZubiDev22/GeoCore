using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Globalization;
using System.Linq;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/rentals")]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalRepository _rentalRepo;
        public RentalsController(IRentalRepository rentalRepo)
        {
            _rentalRepo = rentalRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalDto>>> GetAll()
        {
            var rentals = await _rentalRepo.GetAllAsync();
            var dtos = rentals.Select(r => new RentalDto
            {
                RentalId = r.RentalId,
                ApartmentId = r.ApartmentId,
                StartDate = r.StartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                EndDate = r.EndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                IsConfirmed = r.IsConfirmed,
                Price = r.Price,
                Zone = r.Zone,
                PostalCode = r.PostalCode
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDto>> GetById(int id)
        {
            var rental = await _rentalRepo.GetByIdAsync(id);
            if (rental == null)
                return NotFound();
            var dto = new RentalDto
            {
                RentalId = rental.RentalId,
                ApartmentId = rental.ApartmentId,
                StartDate = rental.StartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                EndDate = rental.EndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                IsConfirmed = rental.IsConfirmed,
                Price = rental.Price,
                Zone = rental.Zone,
                PostalCode = rental.PostalCode
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RentalDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var entity = new Rental
            {
                ApartmentId = dto.ApartmentId,
                StartDate = DateTime.ParseExact(dto.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                EndDate = DateTime.ParseExact(dto.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                IsConfirmed = dto.IsConfirmed,
                Price = dto.Price,
                Zone = dto.Zone,
                PostalCode = dto.PostalCode
            };
            await _rentalRepo.AddAsync(entity);
            return Ok();
        }

        [HttpGet("by-building-code/{code}")]
        public async Task<ActionResult<IEnumerable<RentalDto>>> GetByBuildingCode(string code)
        {
            // Obtener el edificio por código
            var buildingRepo = HttpContext.RequestServices.GetService<IBuildingRepository>();
            var apartmentRepo = HttpContext.RequestServices.GetService<IApartmentRepository>();
            if (buildingRepo == null || apartmentRepo == null)
                return StatusCode(500, "Dependencias no disponibles");
            var building = await buildingRepo.GetByCodeAsync(code);
            if (building == null)
                return NotFound($"No se encontró el edificio con código {code}");
            var apartments = await apartmentRepo.GetAllAsync();
            var apartmentIds = apartments.Where(a => a.BuildingId == building.BuildingId).Select(a => a.ApartmentId).ToList();
            var rentals = await _rentalRepo.GetAllAsync();
            var dtos = rentals.Where(r => apartmentIds.Contains(r.ApartmentId)).Select(r => new RentalDto
            {
                RentalId = r.RentalId,
                ApartmentId = r.ApartmentId,
                StartDate = r.StartDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                EndDate = r.EndDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                IsConfirmed = r.IsConfirmed,
                Price = r.Price,
                Zone = r.Zone,
                PostalCode = r.PostalCode
            });
            return Ok(dtos);
        }
    }
}
