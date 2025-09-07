using System.ComponentModel.DataAnnotations;

namespace GeoCore.DTOs
{
    public class RentalDto
    {
        public string RentalId { get; set; } = string.Empty;
        [Required]
        public string ApartmentId { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$")]
        public string StartDate { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$")]
        public string EndDate { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? Zone { get; set; }
        public string? PostalCode { get; set; }
    }
}
