using AddressManagement.Application.Repositories;
using AddressManagement.Domain;
using AddressManagement.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace AddressManagement.Infrastructure.Repositories;

public class LocationRepository(AddressDbContext dbContext) : ILocationRepository
{
    public Task<Location?> Find(int countryId, string zipCode, string name, CancellationToken ct) =>
        dbContext.Locations.FirstOrDefaultAsync(
            l => l.CountryId == countryId && l.ZipCode == zipCode && l.Name == name, ct);
}