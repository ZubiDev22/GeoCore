using System.ComponentModel.DataAnnotations;

namespace GeoCore.DTOs
{
    public class CashFlowDto
    {
        [Required]
        [MaxLength(10)]
        public string CashFlowId { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
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
