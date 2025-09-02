namespace GeoCore.Entities
{
    public class ManagementBudget
    {
        public string ManagementBudgetId { get; set; } = string.Empty; // MBG001, MBG002, ...
        public string BuildingCode { get; set; } = string.Empty; // BLD001, ...
        public DateTime Date { get; set; }
        public decimal Profitability { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
    }
}
