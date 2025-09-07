// Validador para BuildingDto
using FluentValidation;
using GeoCore.DTOs;

namespace GeoCore.Validators
{
    public class BuildingDtoValidator : AbstractValidator<BuildingDto>
    {
        public BuildingDtoValidator()
        {
            // Elimina la regla de validación para BuildingId como entero
            RuleFor(x => x.BuildingCode).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
            RuleFor(x => x.City).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
            RuleFor(x => x.PurchaseDate).NotEmpty().Matches(@"^\d{2}/\d{2}/\d{4}$");
            RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
        }
    }
}
