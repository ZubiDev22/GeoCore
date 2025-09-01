namespace GeoCore.Entities
{
    public class MaintenanceEvent
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
