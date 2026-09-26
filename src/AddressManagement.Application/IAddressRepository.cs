using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

namespace AddressManagement.Application;

// Writes work with entities, reads project directly to DTOs (no over-fetching).
public interface IAddressRepository
{
    Task<PagedResult<AddressListDto>> GetAll(AddressQueryDto queryDto, CancellationToken ct);
    Task<Address?> GetById(int id, CancellationToken ct);
    Task Add(Address address, CancellationToken ct);
    Task Update(Address address, CancellationToken ct);
    public Task<bool> Exists(AddressCreateDto dto, int? excludeId, CancellationToken ct);
    Task<bool> Delete(int id, CancellationToken ct);
    Task<int> DeleteMany(int[] ids, CancellationToken ct);
    Task<IEnumerable<CountryDto>> GetCountries(CancellationToken ct);
    Task<Recipient?> FindRecipient(string name, CancellationToken ct);
    Task<Country?> FindCountry(string name, CancellationToken ct);
    Task<Location?> FindLocation(int countryId, string zipCode, string name, CancellationToken ct);
}
