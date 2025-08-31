namespace GeoCore.DTOs
{
    public class CashFlowDto
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; } = string.Empty;
    }

    public class MaintenanceEventDto
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }

    public class AssetAssessmentDto
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public DateTime Date { get; set; }
        public decimal Profitability { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
    }
}
