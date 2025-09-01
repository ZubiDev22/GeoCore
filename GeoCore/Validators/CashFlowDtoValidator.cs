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
}
