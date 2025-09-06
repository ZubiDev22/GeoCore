using FluentValidation;
using GeoCore.DTOs;

namespace GeoCore.Validators
{
    public class MaintenanceEventDtoValidator : AbstractValidator<MaintenanceEventDto>
    {
        public MaintenanceEventDtoValidator()
        {
            RuleFor(x => x.MaintenanceEventCode).NotEmpty().MaximumLength(10);
            RuleFor(x => x.BuildingId).GreaterThan(0);
            RuleFor(x => x.Date).NotEmpty().Matches(@"^\d{2}/\d{2}/\d{4}$");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0);
        }
    }
}
