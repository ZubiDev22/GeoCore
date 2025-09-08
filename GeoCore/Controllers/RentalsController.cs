using GeoCore.Application.Common;
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
        public async Task<ActionResult<Result<IEnumerable<RentalDto>>>> GetAll()
        {
            try
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
                return Ok(Result<IEnumerable<RentalDto>>.Success(dtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Result<IEnumerable<RentalDto>>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<RentalDto>>> GetById(string id)
        {
            try
            {
                var rental = (await _rentalRepo.GetAllAsync()).FirstOrDefault(r => r.RentalId == id);
                if (rental == null)
                    return NotFound(Result<RentalDto>.Failure(new NotFoundError($"Rental with id '{id}' not found.")));
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
                return Ok(Result<RentalDto>.Success(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Result<RentalDto>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }

        [HttpPost]
        public async Task<ActionResult<Result<object>>> Create([FromBody] RentalDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(Result<object>.Failure(new ValidationError("Modelo de datos inválido.")));
            try
            {
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
                return Ok(Result<object>.Success(new { entity.RentalId }));
            }
            catch (FormatException)
            {
                return BadRequest(Result<object>.Failure(new ValidationError("Formato de fecha inválido.")));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Result<object>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }

        [HttpGet("by-building-code/{code}")]
        public async Task<ActionResult<Result<IEnumerable<RentalDto>>>> GetByBuildingCode(string code)
        {
            try
            {
                var buildingRepo = HttpContext.RequestServices.GetService<IBuildingRepository>();
                var apartmentRepo = HttpContext.RequestServices.GetService<IApartmentRepository>();
                if (buildingRepo == null || apartmentRepo == null)
                    return StatusCode(500, Result<IEnumerable<RentalDto>>.Failure(new DataAccessError("Dependencias no disponibles")));
                var building = await buildingRepo.GetByCodeAsync(code);
                if (building == null)
                    return NotFound(Result<IEnumerable<RentalDto>>.Failure(new NotFoundError($"No se encontró el edificio con código {code}")));
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
                return Ok(Result<IEnumerable<RentalDto>>.Success(dtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Result<IEnumerable<RentalDto>>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }

        [HttpGet("{id}/rental-comparison")]
        public async Task<ActionResult<Result<object>>> GetRentalComparison(string id)
        {
            try
            {
                var rental = (await _rentalRepo.GetAllAsync()).FirstOrDefault(r => r.RentalId == id);
                if (rental == null)
                    return NotFound(Result<object>.Failure(new NotFoundError("Rental not found.")));
                var apartmentRepo = HttpContext.RequestServices.GetService<IApartmentRepository>();
                if (apartmentRepo == null)
                    return StatusCode(500, Result<object>.Failure(new DataAccessError("Repositorio de apartamentos no disponible")));
                var apartment = (await apartmentRepo.GetAllAsync()).FirstOrDefault(a => a.ApartmentId == rental.ApartmentId);
                if (apartment == null)
                    return NotFound(Result<object>.Failure(new NotFoundError("No se encontró el apartamento asociado.")));
                var buildingRepo = HttpContext.RequestServices.GetService<IBuildingRepository>();
                if (buildingRepo == null)
                    return StatusCode(500, Result<object>.Failure(new DataAccessError("Repositorio de edificios no disponible")));
                var building = await buildingRepo.GetByIdAsync(apartment.BuildingId);
                if (building == null)
                    return NotFound(Result<object>.Failure(new NotFoundError("No se encontró el edificio asociado.")));
                decimal? averagePrice = null;
                string criterio = "";
                if (!string.IsNullOrWhiteSpace(rental.PostalCode) &&
                    GeoCore.Seeders.RentalPriceSeeder.AverageRentalPricesByCity.TryGetValue(rental.PostalCode, out var avgByCP))
                {
                    averagePrice = avgByCP;
                    criterio = $"Código Postal {rental.PostalCode}";
                }
                else if (!string.IsNullOrWhiteSpace(rental.Zone) &&
                    GeoCore.Seeders.RentalPriceSeeder.AverageRentalPricesByCity.TryGetValue(rental.Zone, out var avgByZone))
                {
                    averagePrice = avgByZone;
                    criterio = $"Zona {rental.Zone}";
                }
                else if (GeoCore.Seeders.RentalPriceSeeder.AverageRentalPricesByCity.TryGetValue(building.City, out var avgByCity))
                {
                    averagePrice = avgByCity;
                    criterio = $"Ciudad {building.City}";
                }
                if (averagePrice == null)
                    return NotFound(Result<object>.Failure(new NotFoundError("No hay datos de precio medio para la localización del alquiler.")));
                var diff = rental.Price - averagePrice.Value;
                var percent = (diff / averagePrice.Value) * 100;
                var percentFormatted = percent.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "%";
                string comparison;
                if (percent > 10) comparison = "Por encima de la media";
                else if (percent < -10) comparison = "Por debajo de la media";
                else comparison = "En la media";
                return Ok(Result<object>.Success(new
                {
                    RentalId = rental.RentalId,
                    ApartmentId = rental.ApartmentId,
                    Price = rental.Price,
                    AveragePrice = averagePrice,
                    Criterio = criterio,
                    Difference = diff,
                    Percentage = percentFormatted,
                    Comparison = comparison
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Result<object>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }
    }
}
