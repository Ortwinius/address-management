using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

namespace AddressManagement.Application.Repositories;

public interface ICountryRepository
{
    Task<IReadOnlyList<CountryDto>> GetAll(CancellationToken ct);
    Task<Country?> FindByName(string name, CancellationToken ct);
}