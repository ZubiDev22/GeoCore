namespace GeoCore.Entities
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int ApartmentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal Price { get; set; }
        public string? Zone { get; set; }
        public string? PostalCode { get; set; }
    }
}
