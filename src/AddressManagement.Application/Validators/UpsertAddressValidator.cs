using AddressManagement.Application.Dtos;

using FluentValidation;

namespace AddressManagement.Application.Validators;

public class UpsertAddressValidator : AbstractValidator<AddressUpsertDto>
{
    public UpsertAddressValidator()
    {
        RuleFor(x => x.Street).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ZipCode).NotEmpty().MaximumLength(10);
    }
}