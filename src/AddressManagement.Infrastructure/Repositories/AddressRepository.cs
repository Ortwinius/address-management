using AddressManagement.Application;
using AddressManagement.Application.Dtos;
using AddressManagement.Application.Mappers;
using AddressManagement.Domain;
using AddressManagement.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace AddressManagement.Infrastructure.Repositories;

public class AddressRepository(AddressDbContext dbContext) : IAddressRepository
{
    public async Task<PagedResult<AddressListDto>> GetAll(int page, int pageSize, CancellationToken ct)
    {
        var query = dbContext.Addresses.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(a => a.Id) // stable order is required for Skip/Take
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(AddressMapper.ToListItem)
            .ToListAsync(ct);

        return new PagedResult<AddressListDto>(items, total, page, pageSize);
    }

    // Tracked on purpose: Update modifies the returned entity.
    public Task<Address?> GetById(int id, CancellationToken ct) =>
        dbContext.Addresses
            .AsNoTracking()
            .Include(a => a.Location)
            .ThenInclude(l => l.Country)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    
    public async Task Add(Address address, CancellationToken ct)
    {
        dbContext.Addresses.Add(address);
        await dbContext.SaveChangesAsync(ct);
    }
    
    public async Task Update(Address address, CancellationToken ct)
    {
        dbContext.Addresses.Update(address);
        await dbContext.SaveChangesAsync(ct);
    }
    
    public async Task Delete(int id, CancellationToken ct)
    {
        var address = await dbContext.Addresses.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (address == null)
        {
            return;
        }
        
        dbContext.Addresses.Remove(address);
        await dbContext.SaveChangesAsync(ct);
    }
    public Task<Country?> FindCountry(string name, CancellationToken ct) =>
        dbContext.Countries.FirstOrDefaultAsync(c => c.Name == name, ct);

    public Task<Location?> FindLocation(int countryId, string zipCode, string name, CancellationToken ct) =>
        dbContext.Locations.FirstOrDefaultAsync(
            l => l.CountryId == countryId && l.ZipCode == zipCode && l.Name == name, ct);

    public Task SaveChanges(CancellationToken ct) => dbContext.SaveChangesAsync(ct);
}
