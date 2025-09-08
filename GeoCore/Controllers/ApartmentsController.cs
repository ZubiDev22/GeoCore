using GeoCore.Application.Common;
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
        public async Task<ActionResult<Result<IEnumerable<ApartmentDto>>>> GetAll()
        {
            try
            {
                var apartments = await _apartmentRepo.GetAllAsync();
                var dtos = apartments.Select(a => new ApartmentDto
                {
                    ApartmentId = a.ApartmentId,
                    ApartmentDoor = a.ApartmentDoor,
                    ApartmentFloor = a.ApartmentFloor,
                    ApartmentPrice = a.ApartmentPrice,
                    NumberOfRooms = a.NumberOfRooms,
                    NumberOfBathrooms = a.NumberOfBathrooms,
                    BuildingId = a.BuildingId,
                    HasLift = a.HasLift,
                    HasGarage = a.HasGarage,
                    CreatedDate = a.CreatedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                });
                return Ok(Result<IEnumerable<ApartmentDto>>.Success(dtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Result<IEnumerable<ApartmentDto>>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<ApartmentDto>>> GetById(string id)
        {
            try
            {
                var apartment = (await _apartmentRepo.GetAllAsync()).FirstOrDefault(a => a.ApartmentId == id);
                if (apartment == null)
                    return NotFound(Result<ApartmentDto>.Failure(new NotFoundError($"Apartment with id '{id}' not found.")));
                var dto = new ApartmentDto
                {
                    ApartmentId = apartment.ApartmentId,
                    ApartmentDoor = apartment.ApartmentDoor,
                    ApartmentFloor = apartment.ApartmentFloor,
                    ApartmentPrice = apartment.ApartmentPrice,
                    NumberOfRooms = apartment.NumberOfRooms,
                    NumberOfBathrooms = apartment.NumberOfBathrooms,
                    BuildingId = apartment.BuildingId,
                    HasLift = apartment.HasLift,
                    HasGarage = apartment.HasGarage,
                    CreatedDate = apartment.CreatedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                };
                return Ok(Result<ApartmentDto>.Success(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Result<ApartmentDto>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }

        [HttpGet("by-building-code/{code}")]
        public async Task<ActionResult<Result<IEnumerable<ApartmentDto>>>> GetByBuildingCode(string code)
        {
            try
            {
                var buildingRepo = HttpContext.RequestServices.GetService<IBuildingRepository>();
                if (buildingRepo == null)
                    return StatusCode(500, Result<IEnumerable<ApartmentDto>>.Failure(new DataAccessError("Repositorio de edificios no disponible")));
                var building = await buildingRepo.GetByCodeAsync(code);
                if (building == null)
                    return NotFound(Result<IEnumerable<ApartmentDto>>.Failure(new NotFoundError($"No se encontró el edificio con código {code}")));
                var apartments = await _apartmentRepo.GetAllAsync();
                var dtos = apartments.Where(a => a.BuildingId == building.BuildingId).Select(a => new ApartmentDto
                {
                    ApartmentId = a.ApartmentId,
                    ApartmentDoor = a.ApartmentDoor,
                    ApartmentFloor = a.ApartmentFloor,
                    ApartmentPrice = a.ApartmentPrice,
                    NumberOfRooms = a.NumberOfRooms,
                    NumberOfBathrooms = a.NumberOfBathrooms,
                    BuildingId = a.BuildingId,
                    HasLift = a.HasLift,
                    HasGarage = a.HasGarage,
                    CreatedDate = a.CreatedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                });
                return Ok(Result<IEnumerable<ApartmentDto>>.Success(dtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Result<IEnumerable<ApartmentDto>>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }
    }
}
