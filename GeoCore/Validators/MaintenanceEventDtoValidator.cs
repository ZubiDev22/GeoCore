using FluentValidation;
using GeoCore.DTOs;

namespace GeoCore.Validators
{
    public class MaintenanceEventDtoValidator : AbstractValidator<MaintenanceEventDto>
    {
        public MaintenanceEventDtoValidator()
        {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0);
        }
    }
}
