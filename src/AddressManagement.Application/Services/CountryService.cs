using AddressManagement.Application.Dtos;
using AddressManagement.Application.Repositories;

namespace AddressManagement.Application.Services;

public class CountryService(ICountryRepository countryRepository) : ICountryService
{
    public Task<IReadOnlyList<CountryDto>> GetAll(CancellationToken ct) => countryRepository.GetAll(ct);
}