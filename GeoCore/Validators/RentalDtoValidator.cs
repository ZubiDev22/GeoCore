using FluentValidation;
using GeoCore.DTOs;

namespace GeoCore.Validators
{
    public class RentalDtoValidator : AbstractValidator<RentalDto>
    {
        public RentalDtoValidator()
        {
            RuleFor(x => x.ApartmentId).GreaterThan(0);
            RuleFor(x => x.StartDate).NotEmpty().Matches(@"^\d{2}/\d{2}/\d{4}$");
            RuleFor(x => x.EndDate).NotEmpty().Matches(@"^\d{2}/\d{2}/\d{4}$");
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
