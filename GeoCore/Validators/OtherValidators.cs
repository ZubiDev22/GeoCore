using FluentValidation;
using GeoCore.DTOs;

namespace GeoCore.Validators
{
    public class CashFlowDtoValidator : AbstractValidator<CashFlowDto>
    {
        public CashFlowDtoValidator()
        {
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Source).NotEmpty().MaximumLength(50);
        }
    }

    public class MaintenanceEventDtoValidator : AbstractValidator<MaintenanceEventDto>
    {
        public MaintenanceEventDtoValidator()
        {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0);
        }
    }

    public class AssetAssessmentDtoValidator : AbstractValidator<AssetAssessmentDto>
    {
        public AssetAssessmentDtoValidator()
        {
            RuleFor(x => x.Profitability).InclusiveBetween(0, 100);
            RuleFor(x => x.RiskLevel).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Recommendation).MaximumLength(200);
        }
    }
}
