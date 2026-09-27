using AddressManagement.Application.Dtos;
using AddressManagement.Application.Mappers;
using AddressManagement.Application.Repositories;
using AddressManagement.Domain;
using AddressManagement.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace AddressManagement.Infrastructure.Repositories;

public class CountryRepository(AddressDbContext dbContext) : ICountryRepository
{
    public async Task<IReadOnlyList<CountryDto>> GetAll(CancellationToken ct) =>
        await dbContext.Countries
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(CountryMapper.ToDto)
            .ToListAsync(ct);

    public Task<Country?> FindByName(string name, CancellationToken ct) =>
        dbContext.Countries.FirstOrDefaultAsync(c => c.Name == name, ct);
}