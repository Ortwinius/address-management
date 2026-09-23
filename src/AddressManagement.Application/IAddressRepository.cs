using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

namespace AddressManagement.Application;

// Writes work with entities, reads project directly to DTOs (no over-fetching).
public interface IAddressRepository
{
    Task<List<AddressListDto>> GetAll(CancellationToken ct);
    Task<List<AddressDetailDto>> GetById(int id, CancellationToken ct);
    Task<Country?> FindCountry(string name, CancellationToken ct);
    Task<Location?> FindLocation(int countryId, string zipCode, string name, CancellationToken ct);
    Task Add(Address address, CancellationToken ct);
}
