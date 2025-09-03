namespace GeoCore.Entities
{
    public class CashFlow
    {
        public int CashFlowId { get; set; }
        public string CashFlowCode { get; set; } = string.Empty;
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}
