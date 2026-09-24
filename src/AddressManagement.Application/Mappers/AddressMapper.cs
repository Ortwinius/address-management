using System.Linq.Expressions;

using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

namespace AddressManagement.Application.Mappers;
public static class AddressMapper
{
    public static readonly Expression<Func<Address, AddressListDto>> ToListItem =
        a => new AddressListDto(a.Id, a.Street, a.Location.ZipCode, a.Location.Name, a.Location.Country.Name);

    public static AddressDetailDto ToDetail(Address a) =>
        new(a.Id, a.Street, a.Location.ZipCode, a.Location.Name, a.Location.Country.Name, a.Recipient, a.AddressAffix);
}
