using System.ComponentModel.DataAnnotations;

namespace GeoCore.DTOs
{
    public class MaintenanceEventDto
    {
        public string MaintenanceEventId { get; set; } = string.Empty;

        public string BuildingId { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$")]
        public string Date { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Cost { get; set; }
    }
}
