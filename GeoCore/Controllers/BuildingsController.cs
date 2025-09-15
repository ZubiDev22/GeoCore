using Microsoft.AspNetCore.Mvc;
using GeoCore.DTOs;
using GeoCore.Repositories;
using GeoCore.Entities;
using System.Linq;
using System.Globalization;
using MediatR;
using GeoCore.Application.Commands;
using GeoCore.Application.Queries;
using System.Collections.Generic;
using GeoCore.Application.Common;
using Microsoft.Extensions.Logging;

namespace GeoCore.Controllers
{
    [ApiController]
    [Route("api/buildings")]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<BuildingsController> _logger;

        public BuildingsController(IBuildingRepository repository, IMediator mediator, ILogger<BuildingsController> logger)
        {
            _repository = repository;
            _mediator = mediator;
            _logger = logger;
        }

        private BuildingDto MapToDto(Building b)
        {
            return new BuildingDto
            {
                BuildingId = b.BuildingId,
                BuildingCode = b.BuildingCode,
                Name = b.Name,
                Address = b.Address,
                City = b.City,
                Latitude = b.Latitude,
                Longitude = b.Longitude,
                PurchaseDate = b.PurchaseDate, // DateTime para formato ISO 8601
                Status = b.Status,
                PostalCode = b.PostalCode
            };
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<BuildingDto>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? city = null,
            [FromQuery] string? status = null)
        {
            _logger.LogInformation($"[BuildingsController] Obteniendo edificios: página {page}, tamaño {pageSize}, city={city}, status={status}");
            var buildings = await _repository.GetAllAsync();
            var query = buildings.AsQueryable();
            if (!string.IsNullOrEmpty(city))
                query = query.Where(b => b.City.ToLower() == city.ToLower());
            if (!string.IsNullOrEmpty(status))
                query = query.Where(b => b.Status.ToLower() == status.ToLower());
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).Select(MapToDto).ToList();
            return Ok(new PagedResultDto<BuildingDto> { Items = items, TotalPages = totalPages });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BuildingDto>> GetById(int id)
        {
            var building = await _repository.GetByIdAsync(id.ToString());
            if (building == null)
                return NotFound();
            var dto = MapToDto(building);
            return Ok(dto);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<BuildingDto>> GetByCode(string code)
        {
            try
            {
                var building = await _repository.GetByCodeAsync(code);
                if (building == null)
                    return NotFound(Result<BuildingDto>.Failure(new NotFoundError($"Building with code '{code}' not found.")));
                var dto = MapToDto(building);
                return Ok(Result<BuildingDto>.Success(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[BuildingsController] Error al obtener el edificio con código {code}");
                return StatusCode(500, Result<BuildingDto>.Failure(new UnexpectedError($"Unexpected error: {ex.Message}")));
            }
        }

        [HttpGet("code/{code}/details")]
        public async Task<ActionResult<BuildingDetailsDto>> GetDetailsByCode(string code)
        {
            var building = await _repository.GetByCodeAsync(code);
            if (building == null)
                return NotFound();
            var status = building.Status;
            string description = "Edificio ingresado en el sistema";
            // Si hay MaintenanceEvent y status es Under Maintenance
            if (status == "Under Maintenance")
            {
                var maintenanceRepo = HttpContext.RequestServices.GetService<IMaintenanceEventRepository>();
                if (maintenanceRepo == null)
                {
                    _logger.LogError("[BuildingsController] IMaintenanceEventRepository no disponible");
                    return StatusCode(500, "Dependencia IMaintenanceEventRepository no disponible");
                }
                var events = await maintenanceRepo.GetAllAsync();
                var lastEvent = events.Where(e => e.BuildingId == building.BuildingId).OrderByDescending(e => e.Date).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(lastEvent?.Description))
                    description = lastEvent.Description;
            }
            // Si hay CashFlow y status es Active
            if (status == "Active")
            {
                var cashFlowRepo = HttpContext.RequestServices.GetService<ICashFlowRepository>();
                if (cashFlowRepo == null)
                {
                    _logger.LogError("[BuildingsController] ICashFlowRepository no disponible");
                    return StatusCode(500, "Dependencia ICashFlowRepository no disponible");
                }
                var cashflows = await cashFlowRepo.GetAllAsync();
                var lastFlow = cashflows.Where(c => c.BuildingId == building.BuildingId).OrderByDescending(c => c.Date).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(lastFlow?.Source))
                    description = lastFlow.Source;
            }
            return Ok(new BuildingDetailsDto {
                BuildingId = building.BuildingId,
                BuildingCode = building.BuildingCode,
                Name = building.Name,
                Address = building.Address,
                City = building.City,
                Latitude = building.Latitude,
                Longitude = building.Longitude,
                PurchaseDate = building.PurchaseDate,
                Status = status,
                PostalCode = building.PostalCode,
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
            {
                _logger.LogError($"[BuildingsController] Error de validación al crear edificio: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                return BadRequest(ModelState);
            }
            var result = await _mediator.Send(new CreateBuildingCommand(dto));
            if (!result.IsSuccess)
            {
                if (result.Error is ValidationError)
                {
                    _logger.LogError($"[BuildingsController] Error de validación al crear edificio: {result.Error.Message}");
                    return BadRequest(result.Error.Message);
                }
                if (result.Error is BusinessRuleError)
                {
                    _logger.LogError($"[BuildingsController] Error de regla de negocio al crear edificio: {result.Error.Message}");
                    return Conflict(result.Error.Message);
                }
                if (result.Error is NotFoundError)
                {
                    _logger.LogError($"[BuildingsController] Error NotFound al crear edificio: {result.Error.Message}");
                    return NotFound(result.Error.Message);
                }
                _logger.LogError($"[BuildingsController] Error inesperado al crear edificio: {result.Error.Message}");
                return StatusCode(500, result.Error.Message);
            }
            return Ok(result.Value);
        }

        [HttpPatch("{code}")]
        public async Task<IActionResult> Patch(string code, [FromBody] List<PatchOperation> operations)
        {
            var success = await _mediator.Send(new PatchBuildingCommand(code, operations));
            if (!success)
            {
                _logger.LogError($"[BuildingsController] Error al hacer patch al edificio con código {code}: no encontrado");
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var success = await _mediator.Send(new DeleteBuildingCommand(code));
            if (!success)
            {
                _logger.LogError($"[BuildingsController] Error al eliminar edificio con código {code}: no encontrado");
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("/api/buildings/{code}/apartments")]
        public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetApartmentsByBuildingCode(string code)
        {
            try
            {
                var buildingRepo = HttpContext.RequestServices.GetService<IBuildingRepository>();
                if (buildingRepo == null)
                    return StatusCode(500, "Repositorio de edificios no disponible");
                var building = await buildingRepo.GetByCodeAsync(code);
                if (building == null)
                    return NotFound($"No se encontró el edificio con código {code}");
                var apartmentRepo = HttpContext.RequestServices.GetService<IApartmentRepository>();
                if (apartmentRepo == null)
                    return StatusCode(500, "Repositorio de apartamentos no disponible");
                var apartments = await apartmentRepo.GetAllAsync();
                var result = apartments.Where(a => a.BuildingId == building.BuildingId).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[BuildingsController] Error inesperado al obtener apartamentos para el edificio {code}");
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

        [HttpGet("code/{code}/profitability")]
        public async Task<ActionResult<ProfitabilityDto>> GetProfitabilityByBuildingCode(string code)
        {
            _logger.LogInformation($"[BuildingsController] Calculando rentabilidad para el edificio {code}");
            var building = await _repository.GetByCodeAsync(code);
            if (building == null)
                return NotFound();

            var apartmentRepo = HttpContext.RequestServices.GetService<IApartmentRepository>();
            var rentalRepo = HttpContext.RequestServices.GetService<IRentalRepository>();
            var cashFlowRepo = HttpContext.RequestServices.GetService<ICashFlowRepository>();
            var maintenanceRepo = HttpContext.RequestServices.GetService<IMaintenanceEventRepository>();

            if (apartmentRepo == null || rentalRepo == null || cashFlowRepo == null || maintenanceRepo == null)
                return StatusCode(500, "Dependencias no disponibles");

            var apartments = (await apartmentRepo.GetAllAsync()).Where(a => a.BuildingId == building.BuildingId).ToList();
            var apartmentIds = apartments.Select(a => a.ApartmentId).ToList();
            var rentals = (await rentalRepo.GetAllAsync()).Where(r => apartmentIds.Contains(r.ApartmentId) && r.IsConfirmed);
            var cashflows = (await cashFlowRepo.GetAllAsync()).Where(c => c.BuildingId == building.BuildingId);
            var maintenances = (await maintenanceRepo.GetAllAsync()).Where(m => m.BuildingId == building.BuildingId);

            var ingresos = rentals.Sum(r => r.Price);
            var gastosCashFlow = cashflows.Where(c => c.Source != null && (c.Source.ToLower().Contains("compra") || c.Source.ToLower().Contains("reforma") || c.Source.ToLower().Contains("gasto"))).Sum(c => c.Amount);
            var gastosMantenimiento = maintenances.Sum(m => m.Cost);
            var gastos = gastosCashFlow + gastosMantenimiento;
            var inversion = cashflows.Where(c => c.Source != null && c.Source.ToLower().Contains("compra")).OrderBy(c => c.Date).FirstOrDefault()?.Amount ?? 0;
            var rentabilidad = inversion > 0 ? (ingresos - gastos) / inversion : 0;
            var rentabilidadFormatted = (rentabilidad * 100).ToString("0.##", CultureInfo.InvariantCulture) + "%";

            var dto = new ProfitabilityDto
            {
                BuildingCode = code,
                Ingresos = ingresos,
                Gastos = gastos,
                Inversion = inversion,
                Rentabilidad = rentabilidadFormatted,
                Detalle = new ProfitabilityDetailDto
                {
                    Alquileres = rentals.Select(r => new RentalSummaryDto
                    {
                        RentalId = r.RentalId,
                        ApartmentId = r.ApartmentId,
                        Price = r.Price,
                        StartDate = r.StartDate,
                        EndDate = r.EndDate
                    }),
                    CashFlows = cashflows.Select(c => new CashFlowSummaryDto
                    {
                        CashFlowId = c.CashFlowId,
                        Source = c.Source,
                        Amount = c.Amount,
                        Date = c.Date
                    }),
                    Mantenimientos = maintenances.Select(m => new MaintenanceSummaryDto
                    {
                        MaintenanceEventId = m.MaintenanceEventId,
                        Description = m.Description,
                        Cost = m.Cost,
                        Date = m.Date
                    })
                }
            };
            return Ok(dto);
        }

        [HttpGet("profitability-by-location")]
        public async Task<ActionResult<ProfitabilityByLocationDto>> GetProfitabilityByLocation(
            [FromQuery] string? postalCode = null,
            [FromQuery] string? zone = null,
            [FromQuery] string? city = null)
        {
            _logger.LogInformation($"[BuildingsController] Calculando rentabilidad por localización: postalCode={postalCode}, zone={zone}, city={city}");
            var buildingRepo = _repository;
            var apartmentRepo = HttpContext.RequestServices.GetService<IApartmentRepository>();
            var rentalRepo = HttpContext.RequestServices.GetService<IRentalRepository>();
            var cashFlowRepo = HttpContext.RequestServices.GetService<ICashFlowRepository>();
            var maintenanceRepo = HttpContext.RequestServices.GetService<IMaintenanceEventRepository>();

            if (apartmentRepo == null || rentalRepo == null || cashFlowRepo == null || maintenanceRepo == null)
                return StatusCode(500, "Dependencias no disponibles");

            var buildings = await buildingRepo.GetAllAsync();
            var apartments = await apartmentRepo.GetAllAsync();
            var rentals = await rentalRepo.GetAllAsync();
            var cashflows = await cashFlowRepo.GetAllAsync();
            var maintenances = await maintenanceRepo.GetAllAsync();

            // Asignación dinámica de estado "Rented" si hay alquiler confirmado
            foreach (var building in buildings)
            {
                var buildingApartments = apartments.Where(a => a.BuildingId == building.BuildingId).ToList();
                var buildingApartmentIds = buildingApartments.Select(a => a.ApartmentId).ToList();
                var hasConfirmedRental = rentals.Any(r => buildingApartmentIds.Contains(r.ApartmentId) && r.IsConfirmed);
                if (hasConfirmedRental)
                {
                    building.Status = "Rented";
                }
            }

            // Función para normalizar texto (quitar espacios y pasar a minúsculas)
            string Normalize(string? s) => string.IsNullOrWhiteSpace(s) ? string.Empty : string.Join(" ", s.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries));

            // Filtrado de edificios según prioridad: postalCode > zone > city
            IEnumerable<string> buildingIds = Enumerable.Empty<string>();
            if (!string.IsNullOrWhiteSpace(postalCode))
            {
                var normalizedPostal = Normalize(postalCode);
                _logger.LogDebug($"Filtro postalCode normalizado: '{normalizedPostal}'");
                buildingIds = buildings
                    .Where(b => Normalize(b.PostalCode).Contains(normalizedPostal))
                    .Select(b => b.BuildingId)
                    .ToList();
                _logger.LogDebug($"BuildingIds encontrados por postalCode: {string.Join(",", buildingIds)}");
            }
            else if (!string.IsNullOrWhiteSpace(zone))
            {
                var normalizedZone = Normalize(zone);
                _logger.LogDebug($"Filtro zone normalizado: '{normalizedZone}'");
                var apartmentIds = rentals
                    .Where(r => Normalize(r.Zone).Contains(normalizedZone))
                    .Select(r => r.ApartmentId)
                    .Distinct()
                    .ToList();
                _logger.LogDebug($"ApartmentIds encontrados por zone: {string.Join(",", apartmentIds)}");
                buildingIds = apartments
                    .Where(a => apartmentIds.Contains(a.ApartmentId))
                    .Select(a => a.BuildingId)
                    .Distinct()
                    .ToList();
                _logger.LogDebug($"BuildingIds encontrados por zone: {string.Join(",", buildingIds)}");
            }
            else if (!string.IsNullOrWhiteSpace(city))
            {
                var normalizedCity = Normalize(city);
                _logger.LogDebug($"Filtro city normalizado: '{normalizedCity}'");
                buildingIds = buildings
                    .Where(b => Normalize(b.City).Contains(normalizedCity))
                    .Select(b => b.BuildingId)
                    .ToList();
                _logger.LogDebug($"BuildingIds encontrados por city: {string.Join(",", buildingIds)}");
            }
            else
            {
                return BadRequest("Debe especificar al menos uno de los parámetros: postalCode, zone o city.");
            }

            var filteredBuildings = buildings.Where(b => buildingIds.Contains(b.BuildingId)).ToList();
            _logger.LogDebug($"Total edificios filtrados: {filteredBuildings.Count}");
            if (!filteredBuildings.Any())
                return NotFound("No se encontraron edificios para la localización indicada.");

            var resultados = new List<ProfitabilityByLocationDetailDto>();
            decimal totalIngresos = 0, totalGastos = 0, totalInversion = 0;
            // Precalcular promedios de edificios Rented por localización
            var rentedBuildingsForAvg = new List<ProfitabilityByLocationDetailDto>();
            // Primero, calcular los datos reales para edificios Rented
            foreach (var building in filteredBuildings)
            {
                var buildingApartments = apartments.Where(a => a.BuildingId == building.BuildingId).ToList();
                var buildingApartmentIds = buildingApartments.Select(a => a.ApartmentId).ToList();
                var buildingRentals = rentals.Where(r => buildingApartmentIds.Contains(r.ApartmentId) && r.IsConfirmed);
                var buildingCashflows = cashflows.Where(c => c.BuildingId == building.BuildingId);
                var buildingMaintenances = maintenances.Where(m => m.BuildingId == building.BuildingId);

                decimal ingresos = 0, gastos = 0, inversion = 0;
                string rentabilidadFormatted = "0%";
                string tipoRentabilidad = "Potencial";
                if (building.Status == "Rented")
                {
                    ingresos = buildingRentals.Sum(r => r.Price);
                    var gastosCashFlow = buildingCashflows.Where(c => c.Source != null && (c.Source.ToLower().Contains("compra") || c.Source.ToLower().Contains("reforma") || c.Source.ToLower().Contains("gasto"))).Sum(c => c.Amount);
                    var gastosMantenimiento = buildingMaintenances.Sum(m => m.Cost);
                    gastos = gastosCashFlow + gastosMantenimiento;
                    inversion = buildingCashflows.Where(c => c.Source != null && c.Source.ToLower().Contains("compra")).OrderBy(c => c.Date).FirstOrDefault()?.Amount ?? 0;
                    var rentabilidad = inversion > 0 ? (ingresos - gastos) / inversion : 0;
                    rentabilidadFormatted = (rentabilidad * 100).ToString("0.##", CultureInfo.InvariantCulture) + "%";
                    tipoRentabilidad = "Real";
                    rentedBuildingsForAvg.Add(new ProfitabilityByLocationDetailDto {
                        BuildingId = building.BuildingId,
                        BuildingCode = building.BuildingCode,
                        Name = building.Name,
                        Address = building.Address,
                        City = building.City,
                        Latitude = building.Latitude,
                        Longitude = building.Longitude,
                        PurchaseDate = building.PurchaseDate,
                        Status = building.Status,
                        PostalCode = building.PostalCode,
                        Ingresos = ingresos,
                        Gastos = gastos,
                        Inversion = inversion,
                        Rentabilidad = rentabilidadFormatted,
                        TipoRentabilidad = tipoRentabilidad
                    });
                }
                resultados.Add(new ProfitabilityByLocationDetailDto
                {
                    BuildingId = building.BuildingId,
                    BuildingCode = building.BuildingCode,
                    Name = building.Name,
                    Address = building.Address,
                    City = building.City,
                    Latitude = building.Latitude,
                    Longitude = building.Longitude,
                    PurchaseDate = building.PurchaseDate,
                    Status = building.Status,
                    PostalCode = building.PostalCode,
                    Ingresos = ingresos,
                    Gastos = gastos,
                    Inversion = inversion,
                    Rentabilidad = rentabilidadFormatted,
                    TipoRentabilidad = tipoRentabilidad
                });
                totalIngresos += ingresos;
                totalGastos += gastos;
                totalInversion += inversion;
            }
            // Ahora, para edificios no Rented, calcular rentabilidad potencial usando promedios de la localización
            foreach (var detail in resultados.Where(r => r.TipoRentabilidad == "Potencial"))
            {
                // Buscar promedios de edificios Rented en la misma ciudad
                var avg = rentedBuildingsForAvg.Where(x => x.City == detail.City);
                if (avg.Any())
                {
                    detail.Ingresos = avg.Average(x => x.Ingresos);
                    detail.Gastos = avg.Average(x => x.Gastos);
                    detail.Inversion = avg.Average(x => x.Inversion);
                    var rent = detail.Inversion > 0 ? (detail.Ingresos - detail.Gastos) / detail.Inversion : 0;
                    detail.Rentabilidad = (rent * 100).ToString("0.##", CultureInfo.InvariantCulture) + "%";
                }
            }

            var rentabilidadMedia = totalInversion > 0 ? (totalIngresos - totalGastos) / totalInversion : 0;
            var rentabilidadMediaFormatted = (rentabilidadMedia * 100).ToString("0.##", CultureInfo.InvariantCulture) + "%";

            // Agrupación y cálculo de rentabilidad por zona, ciudad y código postal (solo edificios Rented)
            Func<decimal, string> getBaremo = r => r < 0.03m ? "Baja" : (r < 0.06m ? "Media" : "Alta");
            var rentedBuildings = resultados.Where(r => r.Status == "Rented").ToList();
            var rentabilidadPorZona = rentedBuildings
                .GroupBy(r => r.City + ":" + (r.Address ?? "").Split(' ').FirstOrDefault() ?? "")
                .Select(g => {
                    var totalInv = g.Sum(x => x.Inversion);
                    var totalIng = g.Sum(x => x.Ingresos);
                    var totalGas = g.Sum(x => x.Gastos);
                    var rent = totalInv > 0 ? (totalIng - totalGas) / totalInv : 0;
                    return new ZoneProfitabilityDto {
                        Zone = g.Key,
                        Rentabilidad = (rent * 100).ToString("0.##") + "%",
                        Baremo = getBaremo(rent)
                    };
                }).ToList();
            var rentabilidadPorCiudad = rentedBuildings
                .GroupBy(r => r.City)
                .Select(g => {
                    var totalInv = g.Sum(x => x.Inversion);
                    var totalIng = g.Sum(x => x.Ingresos);
                    var totalGas = g.Sum(x => x.Gastos);
                    var rent = totalInv > 0 ? (totalIng - totalGas) / totalInv : 0;
                    return new CityProfitabilityDto {
                        City = g.Key,
                        Rentabilidad = (rent * 100).ToString("0.##") + "%",
                        Baremo = getBaremo(rent)
                    };
                }).ToList();
            var rentabilidadPorPostalCode = rentedBuildings
                .GroupBy(r => r.PostalCode)
                .Select(g => {
                    var totalInv = g.Sum(x => x.Inversion);
                    var totalIng = g.Sum(x => x.Ingresos);
                    var totalGas = g.Sum(x => x.Gastos);
                    var rent = totalInv > 0 ? (totalIng - totalGas) / totalInv : 0;
                    return new PostalCodeProfitabilityDto {
                        PostalCode = g.Key,
                        Rentabilidad = (rent * 100).ToString("0.##") + "%",
                        Baremo = getBaremo(rent)
                    };
                }).ToList();

            var dto = new ProfitabilityByLocationDto
            {
                TotalEdificios = resultados.Count,
                TotalIngresos = totalIngresos,
                TotalGastos = totalGastos,
                TotalInversion = totalInversion,
                RentabilidadMedia = rentabilidadMediaFormatted,
                Detalle = resultados,
                RentabilidadPorZona = rentabilidadPorZona,
                RentabilidadPorCiudad = rentabilidadPorCiudad,
                RentabilidadPorPostalCode = rentabilidadPorPostalCode,
                EscalaBaremosDescripcion = "Baja < 3%, Media 3-6%, Alta > 6%"
            };
            return Ok(dto);
        }
    }
}
