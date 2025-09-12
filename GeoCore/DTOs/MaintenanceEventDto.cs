using System.ComponentModel.DataAnnotations;

namespace GeoCore.DTOs
{
    public class MaintenanceEventDto
    {
        public string MaintenanceEventId { get; set; } = string.Empty;

        public string BuildingId { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; } // ISO 8601 para Angular

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Cost { get; set; }
    }
}
