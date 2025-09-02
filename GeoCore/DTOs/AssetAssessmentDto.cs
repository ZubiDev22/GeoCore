using System.ComponentModel.DataAnnotations;

namespace GeoCore.DTOs
{
    public class ManagementBudgetDto
    {
        [Required]
        [MaxLength(10)]
        public string ManagementBudgetId { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string BuildingCode { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$")]
        public string Date { get; set; } = string.Empty;
        [Required]
        public decimal Profitability { get; set; }
        [Required]
        [MaxLength(50)]
        public string RiskLevel { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Recommendation { get; set; } = string.Empty;
    }
}
