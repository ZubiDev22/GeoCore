using FluentValidation;
using GeoCore.DTOs;

namespace GeoCore.Validators
{
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
