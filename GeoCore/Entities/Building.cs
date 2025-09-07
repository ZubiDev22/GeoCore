// Entidad principal para edificios
using System.Collections.Generic;

namespace GeoCore.Entities
{
    public class Building
    {
        public string BuildingId { get; set; } = string.Empty; // BLD001, BLD002, ...
        public string BuildingCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; } = string.Empty; // "Active", "Under Maintenance", "Rented"

        // Relaciones de navegación
        public ICollection<CashFlow> CashFlows { get; set; } = new List<CashFlow>();
        public ICollection<MaintenanceEvent> MaintenanceEvents { get; set; } = new List<MaintenanceEvent>();
        public ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();
    }
}
