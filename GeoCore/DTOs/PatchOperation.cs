using System.ComponentModel.DataAnnotations;

namespace GeoCore.DTOs
{
    public class PatchOperation
    {
        [Required]
        [RegularExpression("^(replace|add|remove)$", ErrorMessage = "Operation must be: replace, add, or remove")]
        public string Op { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^/(status|description|amount|source|cashFlowId|buildingCode|date|cost|maintenanceEventId|profitability|riskLevel|recommendation|managementBudgetId)$",
            ErrorMessage = "Path must be a valid field for the target entity")]
        public string Path { get; set; } = string.Empty;

        public object? Value { get; set; }
    }
}
