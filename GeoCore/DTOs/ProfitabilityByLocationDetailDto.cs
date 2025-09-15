using System;

namespace GeoCore.DTOs
{
    public class ProfitabilityByLocationDetailDto
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
        public decimal Ingresos { get; set; }
        public decimal Gastos { get; set; }
        public decimal Inversion { get; set; }
        public string Rentabilidad { get; set; } = string.Empty;
        public string? TipoRentabilidad { get; set; }
        public string? Baremo { get; set; }
        public string Zone { get; set; } = string.Empty;
    }
}