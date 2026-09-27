using AddressManagement.Application.Dtos;

namespace AddressManagement.Application.Services;

public interface ICountryService
{
    Task<IReadOnlyList<CountryDto>> GetAll(CancellationToken ct);
}