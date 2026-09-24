using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

namespace AddressManagement.Application;

// Writes work with entities, reads project directly to DTOs (no over-fetching).
public interface IAddressRepository
{
    Task<PagedResult<AddressListDto>> GetAddresses(AddressQueryDto queryDto, CancellationToken ct);
    Task<IEnumerable<Country>> GetCountries(CancellationToken ct);
    Task<Address?> GetById(int id, CancellationToken ct);
    Task<Country?> FindCountry(string name, CancellationToken ct);
    Task<Location?> FindLocation(int countryId, string zipCode, string name, CancellationToken ct);
    Task Add(Address address, CancellationToken ct);

    Task Update(Address address, CancellationToken ct);
    Task<bool> Delete(int id, CancellationToken ct);
}
