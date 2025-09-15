using System.ComponentModel.DataAnnotations;

namespace GeoCore.DTOs
{
    public class ApartmentDto
    {
        public string ApartmentId { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string ApartmentDoor { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string ApartmentFloor { get; set; } = string.Empty;
        [Required]
        public decimal ApartmentPrice { get; set; }
        [Required]
        public int NumberOfRooms { get; set; }
        [Required]
        public int NumberOfBathrooms { get; set; }
        [Required]
        public string BuildingId { get; set; } = string.Empty;
        public bool HasLift { get; set; }
        public bool HasGarage { get; set; }
        public string CreatedDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
