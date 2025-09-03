
// using System.ComponentModel.DataAnnotations;
// 
// namespace GeoCore.DTOs
// {
//     public class CashFlowDto
//     {
//         public int CashFlowId { get; set; }
//         [Required]
//         [MaxLength(10)]
//         public string CashFlowCode { get; set; } = string.Empty;
//         public int BuildingId { get; set; }
//         [Required]
//         [RegularExpression(@"^\d{2}/\d{2}/\d{4}$")]
//         public string Date { get; set; } = string.Empty;
//         [Required]
//         public decimal Amount { get; set; }
//         [Required]
//         [MaxLength(50)]
//         public string Source { get; set; } = string.Empty;
//     }
// }
