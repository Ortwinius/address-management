using AddressManagement.Application.Dtos;

using FluentValidation;

namespace AddressManagement.Application.Validators;

public class AddressQueryDtoValidator : AbstractValidator<AddressQueryDto>
{
    public AddressQueryDtoValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThan(100);
        RuleFor(x => x.Page)
            .Must((query, page) => (long)page * query.PageSize <= AddressQueryDto.MaxResults)
            .WithMessage($"Only the first {AddressQueryDto.MaxResults} results can be paged. Narrow the filter instead.");
        RuleFor(x => x.Countries!.Length).LessThan(300).When(x => x.Countries is not null);

        // Shorter search terms can't use the street index and would scan all addresses.
        RuleFor(x => x.Street)
            .MaximumLength(FieldLimits.Text)
            .Must(street => street!.Trim().Length >= AddressQueryDto.MinStreetSearchLength)
            .WithMessage($"'{{PropertyName}}' needs at least {AddressQueryDto.MinStreetSearchLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Street));
        RuleFor(x => x.Location).MaximumLength(FieldLimits.Text);
    }
}
