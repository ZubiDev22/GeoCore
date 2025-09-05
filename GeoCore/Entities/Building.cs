// Entidad principal para edificios
using System.Collections.Generic;

namespace GeoCore.Entities
{
    public class Building
    {
        public int BuildingId { get; set; }
        public string BuildingCode { get; set; } = string.Empty; // BLD001, BLD002, ...
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
        public ICollection<ManagementBudget> ManagementBudgets { get; set; } = new List<ManagementBudget>();
        public ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();
    }
}
