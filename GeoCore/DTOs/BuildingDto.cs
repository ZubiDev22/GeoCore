using System.Text.Json.Serialization;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

// DTO para Building
namespace GeoCore.DTOs
{
    public class BuildingDto
    {
        public string BuildingId { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string BuildingCode { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Required]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$")]
        public string PurchaseDate { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }
}
