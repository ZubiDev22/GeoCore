using FluentValidation;
using GeoCore.DTOs;

namespace GeoCore.Validators
{
    public class ManagementBudgetDtoValidator : AbstractValidator<ManagementBudgetDto>
    {
        public ManagementBudgetDtoValidator()
        {
            RuleFor(x => x.ManagementBudgetId).NotEmpty().MaximumLength(10);
            RuleFor(x => x.BuildingCode).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Profitability).InclusiveBetween(0, 100);
            RuleFor(x => x.RiskLevel).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Recommendation).MaximumLength(200);
        }
    }
}
