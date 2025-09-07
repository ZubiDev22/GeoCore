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
using System.Collections.Generic;

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
            var buildings = await _repository.GetAllAsync();
            var dtos = buildings.Skip((page - 1) * pageSize).Take(pageSize).Select(b => new BuildingDto
            {
                BuildingId = b.BuildingId,
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BuildingDto>> GetById(int id)
        {
            var building = await _repository.GetByIdAsync(id);
            if (building == null)
                return NotFound();
            var dto = new BuildingDto
            {
                BuildingId = building.BuildingId,
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

        [HttpGet("code/{code}")]
        public async Task<ActionResult<BuildingDto>> GetByCode(string code)
        {
            var building = await _repository.GetByCodeAsync(code);
            if (building == null)
                return NotFound();
            var dto = new BuildingDto
            {
                BuildingId = building.BuildingId,
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

        [HttpGet("code/{code}/details")]
        public async Task<ActionResult<object>> GetDetailsByCode(string code)
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
                if (maintenanceRepo != null)
                {
                    var events = await maintenanceRepo.GetAllAsync();
                    var lastEvent = events.Where(e => e.BuildingId == building.BuildingId).OrderByDescending(e => e.Date).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(lastEvent?.Description))
                        description = lastEvent.Description;
                }
            }
            // Si hay CashFlow y status es Active
            if (status == "Active")
            {
                var cashFlowRepo = HttpContext.RequestServices.GetService<ICashFlowRepository>();
                if (cashFlowRepo != null)
                {
                    var cashflows = await cashFlowRepo.GetAllAsync();
                    var lastFlow = cashflows.Where(c => c.BuildingId == building.BuildingId).OrderByDescending(c => c.Date).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(lastFlow?.Source))
                        description = lastFlow.Source;
                }
            }
            // Si hay ManagementBudget y status es Rented
            // if (status == "Rented")
            // {
            //     var budgetRepo = HttpContext.RequestServices.GetService<IManagementBudgetRepository>();
            //     if (budgetRepo != null)
            //     {
            //         var budgets = await budgetRepo.GetAllAsync();
            //         var lastBudget = budgets.Where(b => b.BuildingId == building.BuildingId).OrderByDescending(b => b.Date).FirstOrDefault();
            //         if (!string.IsNullOrWhiteSpace(lastBudget?.Recommendation))
            //             description = lastBudget.Recommendation;
            //     }
            // }
            return Ok(new {
                BuildingId = building.BuildingId,
                BuildingCode = building.BuildingCode,
                Name = building.Name,
                Address = building.Address,
                City = building.City,
                Latitude = building.Latitude,
                Longitude = building.Longitude,
                PurchaseDate = building.PurchaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = status,
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
                return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateBuildingCommand(dto));
            return Ok(result);
        }

        [HttpPatch("{code}")]
        public async Task<IActionResult> Patch(string code, [FromBody] List<PatchOperation> operations)
        {
            var success = await _mediator.Send(new PatchBuildingCommand(code, operations));
            if (!success)
                return NotFound();
            return Ok();
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var success = await _mediator.Send(new DeleteBuildingCommand(code));
            if (!success)
                return NotFound();
            return NoContent();
        }

        [HttpGet("/api/buildings/{code}/apartments")]
        public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetApartmentsByBuildingCode(string code)
        {
            var result = await _mediator.Send(new GetApartmentsByBuildingCodeQuery(code));
            return Ok(result);
        }

        [HttpGet("code/{code}/profitability")]
        public async Task<ActionResult<object>> GetProfitabilityByBuildingCode(string code)
        {
            _loguer.LogInfo($"Calculando rentabilidad para el edificio {code}");
            var building = await _repository.GetByCodeAsync(code);
            if (building == null)
                return NotFound();

            // Obtener apartamentos del edificio
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

            // Ingresos: suma de precios de alquiler confirmados
            var ingresos = rentals.Sum(r => r.Price);
            // Gastos: suma de cashflows de tipo gasto y mantenimientos
            var gastosCashFlow = cashflows.Where(c => c.Source != null && (c.Source.ToLower().Contains("compra") || c.Source.ToLower().Contains("reforma") || c.Source.ToLower().Contains("gasto"))).Sum(c => c.Amount);
            var gastosMantenimiento = maintenances.Sum(m => m.Cost);
            var gastos = gastosCashFlow + gastosMantenimiento;
            // Inversión inicial: primer cashflow de tipo compra
            var inversion = cashflows.Where(c => c.Source != null && c.Source.ToLower().Contains("compra")).OrderBy(c => c.Date).FirstOrDefault()?.Amount ?? 0;
            // Rentabilidad
            var rentabilidad = inversion > 0 ? (ingresos - gastos) / inversion : 0;

            return Ok(new
            {
                BuildingCode = code,
                Ingresos = ingresos,
                Gastos = gastos,
                Inversion = inversion,
                Rentabilidad = rentabilidad,
                Detalle = new {
                    Alquileres = rentals.Select(r => new { r.RentalId, r.ApartmentId, r.Price, r.StartDate, r.EndDate }),
                    CashFlows = cashflows.Select(c => new { c.CashFlowId, c.Source, c.Amount, c.Date }),
                    Mantenimientos = maintenances.Select(m => new { m.MaintenanceEventId, m.Description, m.Cost, m.Date })
                }
            });
        }

        [HttpGet("profitability-by-location")]
        public async Task<ActionResult<object>> GetProfitabilityByLocation(
            [FromQuery] string? postalCode = null,
            [FromQuery] string? zone = null,
            [FromQuery] string? city = null)
        {
            _loguer.LogInfo($"Calculando rentabilidad por localización: postalCode={postalCode}, zone={zone}, city={city}");
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

            // Filtrado de edificios según prioridad: postalCode > zone > city
            IEnumerable<int> buildingIds = Enumerable.Empty<int>();
            if (!string.IsNullOrEmpty(postalCode))
            {
                var apartmentIds = rentals.Where(r => r.PostalCode == postalCode).Select(r => r.ApartmentId).Distinct();
                buildingIds = apartments.Where(a => apartmentIds.Contains(a.ApartmentId)).Select(a => a.BuildingId).Distinct();
            }
            else if (!string.IsNullOrEmpty(zone))
            {
                var apartmentIds = rentals.Where(r => r.Zone == zone).Select(r => r.ApartmentId).Distinct();
                buildingIds = apartments.Where(a => apartmentIds.Contains(a.ApartmentId)).Select(a => a.BuildingId).Distinct();
            }
            else if (!string.IsNullOrEmpty(city))
            {
                buildingIds = buildings.Where(b => b.City == city).Select(b => b.BuildingId);
            }
            else
            {
                return BadRequest("Debe especificar al menos uno de los parámetros: postalCode, zone o city.");
            }

            var filteredBuildings = buildings.Where(b => buildingIds.Contains(b.BuildingId)).ToList();
            if (!filteredBuildings.Any())
                return NotFound("No se encontraron edificios para la localización indicada.");

            var resultados = new List<object>();
            decimal totalIngresos = 0, totalGastos = 0, totalInversion = 0;
            foreach (var building in filteredBuildings)
            {
                var buildingApartments = apartments.Where(a => a.BuildingId == building.BuildingId).ToList();
                var buildingApartmentIds = buildingApartments.Select(a => a.ApartmentId).ToList();
                var buildingRentals = rentals.Where(r => buildingApartmentIds.Contains(r.ApartmentId) && r.IsConfirmed);
                var buildingCashflows = cashflows.Where(c => c.BuildingId == building.BuildingId);
                var buildingMaintenances = maintenances.Where(m => m.BuildingId == building.BuildingId);

                var ingresos = buildingRentals.Sum(r => r.Price);
                var gastosCashFlow = buildingCashflows.Where(c => c.Source != null && (c.Source.ToLower().Contains("compra") || c.Source.ToLower().Contains("reforma") || c.Source.ToLower().Contains("gasto"))).Sum(c => c.Amount);
                var gastosMantenimiento = buildingMaintenances.Sum(m => m.Cost);
                var gastos = gastosCashFlow + gastosMantenimiento;
                var inversion = buildingCashflows.Where(c => c.Source != null && c.Source.ToLower().Contains("compra")).OrderBy(c => c.Date).FirstOrDefault()?.Amount ?? 0;
                var rentabilidad = inversion > 0 ? (ingresos - gastos) / inversion : 0;

                totalIngresos += ingresos;
                totalGastos += gastos;
                totalInversion += inversion;

                resultados.Add(new
                {
                    BuildingCode = building.BuildingCode,
                    Ingresos = ingresos,
                    Gastos = gastos,
                    Inversion = inversion,
                    Rentabilidad = rentabilidad
                });
            }

            var rentabilidadMedia = totalInversion > 0 ? (totalIngresos - totalGastos) / totalInversion : 0;

            return Ok(new
            {
                TotalEdificios = resultados.Count,
                TotalIngresos = totalIngresos,
                TotalGastos = totalGastos,
                TotalInversion = totalInversion,
                RentabilidadMedia = rentabilidadMedia,
                Detalle = resultados
            });
        }
    }
}
