namespace GeoCore.Entities
{
    public class Rental
    {
        public string RentalId { get; set; } = string.Empty;
        public string ApartmentId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal Price { get; set; }
        public string? Zone { get; set; }
        public string? PostalCode { get; set; }
    }
}
