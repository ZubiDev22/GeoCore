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

        private async Task<string> GetBuildingStatusFromApartments(string buildingId)
        {
            var apartmentRepo = HttpContext.RequestServices.GetService<IApartmentRepository>();
            if (apartmentRepo == null)
                return string.Empty;
            var apartments = (await apartmentRepo.GetByBuildingIdAsync(buildingId)).ToList();
            if (!apartments.Any())
                return string.Empty; // Mantener el estado actual si no hay apartamentos
            // Si al menos uno en Maintenance
            if (apartments.Any(a => a.GetType().GetProperty("Status")?.GetValue(a)?.ToString() == "Maintenance"))
                return "Maintenance";
            // Si al menos uno en Active
            if (apartments.Any(a => a.GetType().GetProperty("Status")?.GetValue(a)?.ToString() == "Active"))
                return "Active";
            // Si todos en Rented
            if (apartments.All(a => a.GetType().GetProperty("Status")?.GetValue(a)?.ToString() == "Rented"))
                return "Rented";
            return string.Empty;
        }

        private async Task<BuildingDto> MapToDtoWithApartmentStatus(Building b)
        {
            var dto = MapToDto(b);
            var statusFromApts = await GetBuildingStatusFromApartments(b.BuildingId);
            if (!string.IsNullOrEmpty(statusFromApts))
                dto.Status = statusFromApts;
            return dto;
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
            var items = new List<BuildingDto>();
            foreach (var b in query.Skip((page - 1) * pageSize).Take(pageSize))
            {
                items.Add(await MapToDtoWithApartmentStatus(b));
            }
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
                var dto = await MapToDtoWithApartmentStatus(building);
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
            var statusFromApts = await GetBuildingStatusFromApartments(building.BuildingId);
            var status = !string.IsNullOrEmpty(statusFromApts) ? statusFromApts : building.Status;
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
                var result = apartments.Where(a => a.BuildingId == building.BuildingId)
                    .Select(a => new ApartmentDto {
                        ApartmentId = a.ApartmentId,
                        ApartmentDoor = a.ApartmentDoor,
                        ApartmentFloor = a.ApartmentFloor,
                        ApartmentPrice = a.ApartmentPrice,
                        NumberOfRooms = a.NumberOfRooms,
                        NumberOfBathrooms = a.NumberOfBathrooms,
                        BuildingId = a.BuildingId,
                        HasLift = a.HasLift,
                        HasGarage = a.HasGarage,
                        CreatedDate = a.CreatedDate.ToString("yyyy-MM-dd"),
                        Status = a.Status // Usar el estado real del apartamento
                    })
                    .ToList();
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
            // Declarar getBaremo al principio del método
            Func<decimal, string> getBaremo = r => r < 0.03m ? "Baja" : (r < 0.06m ? "Media" : "Alta");

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

            // Función para normalizar texto (quitar espacios y pasar a minúsculas)
            string Normalize(string? s) => string.IsNullOrWhiteSpace(s) ? string.Empty : string.Join(" ", s.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries));

            // Asignar zone representativo a cada edificio
            string GetZoneForBuilding(GeoCore.Entities.Building b)
            {
                if (b.City.ToLower() == "pamplona")
                {
                    if (b.Address.ToLower().Contains("estafeta")) return "Centro";
                    if (b.Address.ToLower().Contains("p?rez goyena") || b.Address.ToLower().Contains("perez goyena")) return "Huarte";
                }
                // Para el resto de edificios, usar el nombre de la ciudad como zona
                return b.City;
            }

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
                buildingIds = buildings
                    .Where(b => Normalize(GetZoneForBuilding(b)).Contains(normalizedZone))
                    .Select(b => b.BuildingId)
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
            // Calcular promedios globales de edificios Rented para rentabilidad potencial
            var rentedBuildings = filteredBuildings.Where(b => b.Status == "Rented").ToList();
            decimal avgIngresos = 0, avgGastos = 0, avgInversion = 0;
            if (rentedBuildings.Any())
            {
                avgIngresos = rentedBuildings.Average(b => {
                    var buildingApartments = apartments.Where(a => a.BuildingId == b.BuildingId).ToList();
                    var buildingApartmentIds = buildingApartments.Select(a => a.ApartmentId).ToList();
                    var buildingRentals = rentals.Where(r => buildingApartmentIds.Contains(r.ApartmentId) && r.IsConfirmed);
                    return buildingRentals.Sum(r => r.Price);
                });
                avgGastos = rentedBuildings.Average(b => {
                    var buildingCashflows = cashflows.Where(c => c.BuildingId == b.BuildingId);
                    var buildingMaintenances = maintenances.Where(m => m.BuildingId == b.BuildingId);
                    var gastosCashFlow = buildingCashflows.Where(c => c.Source != null && (c.Source.ToLower().Contains("compra") || c.Source.ToLower().Contains("reforma") || c.Source.ToLower().Contains("gasto"))).Sum(c => c.Amount);
                    var gastosMantenimiento = buildingMaintenances.Sum(m => m.Cost);
                    return gastosCashFlow + gastosMantenimiento;
                });
                avgInversion = rentedBuildings.Average(b => {
                    var buildingCashflows = cashflows.Where(c => c.BuildingId == b.BuildingId);
                    return buildingCashflows.Where(c => c.Source != null && c.Source.ToLower().Contains("compra")).OrderBy(c => c.Date).FirstOrDefault()?.Amount ?? 0;
                });
            }

            // Lógica original: para cada edificio filtrado, calcular datos reales o dejar en 0 si no hay datos
            foreach (var building in filteredBuildings)
            {
                var buildingApartments = apartments.Where(a => a.BuildingId == building.BuildingId).ToList();
                var buildingApartmentIds = buildingApartments.Select(a => a.ApartmentId).ToList();
                var buildingRentals = rentals.Where(r => buildingApartmentIds.Contains(r.ApartmentId) && r.IsConfirmed);
                var buildingCashflows = cashflows.Where(c => c.BuildingId == building.BuildingId);
                var buildingMaintenances = maintenances.Where(m => m.BuildingId == building.BuildingId);

                decimal ingresos = buildingRentals.Sum(r => r.Price);
                decimal gastosCashFlow = buildingCashflows.Where(c => c.Source != null && (c.Source.ToLower().Contains("compra") || c.Source.ToLower().Contains("reforma") || c.Source.ToLower().Contains("gasto"))).Sum(c => c.Amount);
                decimal gastosMantenimiento = buildingMaintenances.Sum(m => m.Cost);
                decimal gastos = gastosCashFlow + gastosMantenimiento;
                decimal inversion = buildingCashflows.Where(c => c.Source != null && c.Source.ToLower().Contains("compra")).OrderBy(c => c.Date).FirstOrDefault()?.Amount ?? 0;
                decimal rentabilidad = inversion > 0 ? (ingresos - gastos) / inversion : 0;
                string rentabilidadFormatted = (rentabilidad * 100).ToString("0.##", CultureInfo.InvariantCulture) + "%";

                resultados.Add(new GeoCore.DTOs.ProfitabilityByLocationDetailDto
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
                    TipoRentabilidad = null,
                    Baremo = null,
                    Zone = GetZoneForBuilding(building) // Nuevo campo zone coherente
                });
                totalIngresos += ingresos;
                totalGastos += gastos;
                totalInversion += inversion;
            }

            var rentabilidadMedia = totalInversion > 0 ? (totalIngresos - totalGastos) / totalInversion : 0;
            var rentabilidadMediaFormatted = (rentabilidadMedia * 100).ToString("0.##", CultureInfo.InvariantCulture) + "%";

            // Agrupación y cálculo de rentabilidad por zona, ciudad y código postal (solo edificios Rented)
            var rentedDetalles = resultados.Where(r => r.Status == "Rented").ToList();
            var rentabilidadPorZona = rentedDetalles
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
            var rentabilidadPorCiudad = rentedDetalles
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
            var rentabilidadPorPostalCode = rentedDetalles
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

        [HttpGet("zones")]
        public async Task<ActionResult<IEnumerable<object>>> GetZones()
        {
            var buildings = await _repository.GetAllAsync();
            // Reutilizar la lógica de zone del endpoint de rentabilidad
            string GetZoneForBuilding(GeoCore.Entities.Building b)
            {
                if (b.City.ToLower() == "pamplona")
                {
                    if (b.Address.ToLower().Contains("estafeta")) return "Centro";
                    if (b.Address.ToLower().Contains("p?rez goyena") || b.Address.ToLower().Contains("perez goyena")) return "Huarte";
                }
                return b.City;
            }
            var zones = buildings
                .Select(b => new { b.BuildingId, b.Name, Zone = GetZoneForBuilding(b) })
                .ToList();
            var uniqueZones = zones.Select(z => z.Zone).Distinct().OrderBy(z => z).ToList();
            return Ok(new { zones = uniqueZones, buildings = zones });
        }
    }
}
