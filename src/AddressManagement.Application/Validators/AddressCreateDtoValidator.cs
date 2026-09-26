using AddressManagement.Application.Dtos;

using FluentValidation;

namespace AddressManagement.Application.Validators;

public class AddressCreateDtoValidator : AbstractValidator<AddressCreateDto>
{
    public AddressCreateDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Street).NotEmpty().MaximumLength(FieldLimits.Text);
        RuleFor(x => x.ZipCode).NotEmpty().MaximumLength(FieldLimits.Text)
            .Must(zip => zip.All(char.IsAsciiDigit)).WithMessage("'{PropertyName}' may only contain digits.");
        RuleFor(x => x.Location).NotEmpty().MaximumLength(FieldLimits.Text)
            .ChildRules();
        RuleFor(x => x.Country).NotEmpty().MaximumLength(FieldLimits.Text);
        RuleFor(x => x.Recipient).NotEmpty().MaximumLength(FieldLimits.Text);
        RuleFor(x => x.AddressAffix).MaximumLength(FieldLimits.Text);
    }
}
