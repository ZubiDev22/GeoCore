namespace GeoCore.Entities
{
    public class MaintenanceEvent
    {
        public string MaintenanceEventId { get; set; } = string.Empty;
        public string BuildingCode { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
