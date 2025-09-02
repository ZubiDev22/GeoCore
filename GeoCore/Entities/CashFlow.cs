namespace GeoCore.Entities
{
    public class CashFlow
    {
        public string CashFlowId { get; set; } = string.Empty;
        public string BuildingCode { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}
