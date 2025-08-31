namespace GeoCore.Entities
{
    public class CashFlow
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; } = string.Empty;
    }

    public class MaintenanceEvent
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }

    public class AssetAssessment
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public DateTime Date { get; set; }
        public decimal Profitability { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
    }
}
