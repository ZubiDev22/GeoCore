// Validador para BuildingDto
using FluentValidation;
using GeoCore.DTOs;

namespace GeoCore.Validators
{
    public class BuildingDtoValidator : AbstractValidator<BuildingDto>
    {
        public BuildingDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
            RuleFor(x => x.City).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
        }
    }
}
