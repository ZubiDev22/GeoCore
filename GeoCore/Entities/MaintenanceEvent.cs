namespace GeoCore.Entities
{
    public class MaintenanceEvent
    {
        public int MaintenanceEventId { get; set; }
        public string MaintenanceEventCode { get; set; } = string.Empty;
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
