using System.ComponentModel.DataAnnotations;

namespace GeoCore.DTOs
{
    public class CashFlowDto
    {
        public string CashFlowId { get; set; } = string.Empty;
        public string BuildingId { get; set; } = string.Empty;
        public string BuildingCode { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$")]
        public string Date { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [MaxLength(50)]
        public string Source { get; set; } = string.Empty;
    }
}
