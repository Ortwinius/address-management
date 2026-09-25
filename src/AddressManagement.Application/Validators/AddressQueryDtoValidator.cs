using AddressManagement.Application.Dtos;

using FluentValidation;

namespace AddressManagement.Application.Validators;

public class AddressQueryDtoValidator : AbstractValidator<AddressQueryDto>
{
    public AddressQueryDtoValidator()
    {
        RuleFor(x => x.PageSize).LessThan(100);
        RuleFor(x => x.Countries!.Length).LessThan(300).When(x => x.Countries is not null);
    }
}
