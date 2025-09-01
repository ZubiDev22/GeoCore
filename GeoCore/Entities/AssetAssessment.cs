namespace GeoCore.Entities
{
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
