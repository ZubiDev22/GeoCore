namespace GeoCore.Entities
{
    public class ManagementBudget
    {
        public int ManagementBudgetId { get; set; }
        public string ManagementBudgetCode { get; set; } = string.Empty;
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public DateTime Date { get; set; }
        public decimal Profitability { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
    }
}
