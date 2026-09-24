using System.Linq.Expressions;

using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

namespace AddressManagement.Application.Mappers;
public static class CountryMapper
{
    public static readonly Expression<Func<Country, CountryDto>> ToDto =
        a => new CountryDto(a.Name);
}
