namespace GeoCore.Entities
{
    public class Apartment
    {
        public string ApartmentId { get; set; } = string.Empty;
        public string ApartmentDoor { get; set; } = string.Empty;
        public string ApartmentFloor { get; set; } = string.Empty;
        public decimal ApartmentPrice { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string BuildingId { get; set; } = string.Empty;
        public bool HasLift { get; set; }
        public bool HasGarage { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = string.Empty;
        public Building Building { get; set; } = null!;
    }
}
