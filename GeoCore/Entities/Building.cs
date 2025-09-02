// Entidad principal para edificios
namespace GeoCore.Entities
{
    public class Building
    {
        public string BuildingCode { get; set; } = string.Empty; // BLD001, BLD002, ...
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; } = string.Empty; // "Active", "Under Maintenance", "Rented"
    }
}
