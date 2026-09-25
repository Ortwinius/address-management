using AddressManagement.Application.Dtos;

using FluentValidation;

namespace AddressManagement.Application.Validators;

public class AddressCreateDtoValidator : AbstractValidator<AddressCreateDto>
{
    public AddressCreateDtoValidator()
    {
        RuleFor(x => x.Street).NotEmpty().MinimumLength(1);
        RuleFor(x => x.ZipCode).NotEmpty();
        RuleFor(x => x.Location).NotEmpty();
        RuleFor(x => x.Country).NotEmpty();
    }
}
