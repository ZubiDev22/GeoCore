using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using System.Globalization;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/apartments")]
    public class ApartmentsController : ControllerBase
    {
        private readonly IApartmentRepository _apartmentRepo;
        public ApartmentsController(IApartmentRepository apartmentRepo)
        {
            _apartmentRepo = apartmentRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetAll()
        {
            var apartments = await _apartmentRepo.GetAllAsync();
            var dtos = apartments.Select(a => new ApartmentDto
            {
                ApartmentId = a.ApartmentId,
                ApartmentCode = a.ApartmentCode,
                ApartmentDoor = a.ApartmentDoor,
                ApartmentFloor = a.ApartmentFloor,
                ApartmentPrice = a.ApartmentPrice,
                NumberOfRooms = a.NumberOfRooms,
                NumberOfBathrooms = a.NumberOfBathrooms,
                BuildingId = a.BuildingId,
                HasLift = a.HasLift,
                HasGarage = a.HasGarage,
                CreatedDate = a.CreatedDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApartmentDto>> GetById(int id)
        {
            var apartment = (await _apartmentRepo.GetAllAsync()).FirstOrDefault(a => a.ApartmentId == id);
            if (apartment == null)
                return NotFound();
            var dto = new ApartmentDto
            {
                ApartmentId = apartment.ApartmentId,
                ApartmentCode = apartment.ApartmentCode,
                ApartmentDoor = apartment.ApartmentDoor,
                ApartmentFloor = apartment.ApartmentFloor,
                ApartmentPrice = apartment.ApartmentPrice,
                NumberOfRooms = apartment.NumberOfRooms,
                NumberOfBathrooms = apartment.NumberOfBathrooms,
                BuildingId = apartment.BuildingId,
                HasLift = apartment.HasLift,
                HasGarage = apartment.HasGarage,
                CreatedDate = apartment.CreatedDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            };
            return Ok(dto);
        }

        [HttpGet("{id}/rental-comparison")]
        public async Task<ActionResult<object>> GetRentalComparison(int id)
        {
            var apartments = await _apartmentRepo.GetAllAsync();
            var apartment = apartments.FirstOrDefault(a => a.ApartmentId == id);
            if (apartment == null)
                return NotFound();

            // Obtener ciudad del edificio
            var city = apartment.Building?.City ?? string.Empty;
            if (string.IsNullOrEmpty(city))
                return BadRequest("No se puede determinar la ciudad del apartamento.");

            // Obtener precio medio de alquiler de la ciudad
            if (!GeoCore.Seeders.RentalPriceSeeder.AverageRentalPricesByCity.TryGetValue(city, out var averagePrice))
                return NotFound($"No hay datos de precio medio para la ciudad: {city}");

            var myPrice = apartment.ApartmentPrice;
            var diff = myPrice - averagePrice;
            var percent = (diff / averagePrice) * 100;
            string comparison;
            if (percent > 10) comparison = "Por encima de la media";
            else if (percent < -10) comparison = "Por debajo de la media";
            else comparison = "En la media";

            return Ok(new
            {
                ApartmentId = apartment.ApartmentId,
                City = city,
                ApartmentPrice = myPrice,
                AveragePrice = averagePrice,
                Difference = diff,
                Percentage = percent,
                Comparison = comparison
            });
        }

        [HttpGet("by-building-code/{code}")]
    public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetByBuildingCode(string code)
    {
        // Obtener el edificio por código
        var buildingRepo = HttpContext.RequestServices.GetService<IBuildingRepository>();
        if (buildingRepo == null)
            return StatusCode(500, "Repositorio de edificios no disponible");
        var building = await buildingRepo.GetByCodeAsync(code);
        if (building == null)
            return NotFound($"No se encontró el edificio con código {code}");

        var apartments = await _apartmentRepo.GetAllAsync();
        var dtos = apartments.Where(a => a.BuildingId == building.BuildingId).Select(a => new ApartmentDto
        {
            ApartmentId = a.ApartmentId,
            ApartmentCode = a.ApartmentCode,
            ApartmentDoor = a.ApartmentDoor,
            ApartmentFloor = a.ApartmentFloor,
            ApartmentPrice = a.ApartmentPrice,
            NumberOfRooms = a.NumberOfRooms,
            NumberOfBathrooms = a.NumberOfBathrooms,
            BuildingId = a.BuildingId,
            HasLift = a.HasLift,
            HasGarage = a.HasGarage,
            CreatedDate = a.CreatedDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        });
        return Ok(dtos);
    }
    }
}
