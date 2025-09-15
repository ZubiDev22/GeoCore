namespace GeoCore.DTOs
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalPages { get; set; }
    }

    public class BuildingDetailsDto
    {
        public string BuildingId { get; set; } = string.Empty;
        public string BuildingCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class ProfitabilityDto
    {
        public string BuildingCode { get; set; } = string.Empty;
        public decimal Ingresos { get; set; }
        public decimal Gastos { get; set; }
        public decimal Inversion { get; set; }
        public string Rentabilidad { get; set; } = string.Empty;
        public ProfitabilityDetailDto Detalle { get; set; } = new();
    }
    public class ProfitabilityDetailDto
    {
        public IEnumerable<RentalSummaryDto> Alquileres { get; set; } = new List<RentalSummaryDto>();
        public IEnumerable<CashFlowSummaryDto> CashFlows { get; set; } = new List<CashFlowSummaryDto>();
        public IEnumerable<MaintenanceSummaryDto> Mantenimientos { get; set; } = new List<MaintenanceSummaryDto>();
    }
    public class RentalSummaryDto
    {
        public string RentalId { get; set; } = string.Empty;
        public string ApartmentId { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class CashFlowSummaryDto
    {
        public string CashFlowId { get; set; } = string.Empty;
        public string? Source { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
    public class MaintenanceSummaryDto
    {
        public string MaintenanceEventId { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }
    }
    public class ProfitabilityByLocationDto
    {
        public int TotalEdificios { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal TotalInversion { get; set; }
        public string RentabilidadMedia { get; set; } = string.Empty;
        public List<ProfitabilityByLocationDetailDto> Detalle { get; set; } = new();
        public List<ZoneProfitabilityDto> RentabilidadPorZona { get; set; } = new();
        public List<CityProfitabilityDto> RentabilidadPorCiudad { get; set; } = new();
        public List<PostalCodeProfitabilityDto> RentabilidadPorPostalCode { get; set; } = new();
        public string EscalaBaremosDescripcion { get; set; } = "Baja < 3%, Media 3-6%, Alta > 6%";
    }
    public class ZoneProfitabilityDto
    {
        public string Zone { get; set; } = string.Empty;
        public string Rentabilidad { get; set; } = string.Empty;
        public string Baremo { get; set; } = string.Empty;
    }
    public class CityProfitabilityDto
    {
        public string City { get; set; } = string.Empty;
        public string Rentabilidad { get; set; } = string.Empty;
        public string Baremo { get; set; } = string.Empty;
    }
    public class PostalCodeProfitabilityDto
    {
        public string PostalCode { get; set; } = string.Empty;
        public string Rentabilidad { get; set; } = string.Empty;
        public string Baremo { get; set; } = string.Empty;
    }
}
