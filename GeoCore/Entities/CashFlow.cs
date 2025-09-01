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
}
