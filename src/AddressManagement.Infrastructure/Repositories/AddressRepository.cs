using AddressManagement.Application;
using AddressManagement.Application.Dtos;
using AddressManagement.Application.Mappers;
using AddressManagement.Domain;
using AddressManagement.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace AddressManagement.Infrastructure.Repositories;

public class AddressRepository(AddressDbContext dbContext) : IAddressRepository
{
    public async Task<PagedResult<AddressListDto>> GetAll(AddressQueryDto queryDto, CancellationToken ct)
    {
        var addresses = dbContext.Addresses.AsNoTracking().Filter(queryDto);

        // Counting millions of rows is expensive, so stop at MaxResults (+1 tells whether there are more).
        var counted = await addresses.Take(AddressQueryDto.MaxResults + 1).CountAsync(ct);

        var items = await addresses.Sort(queryDto)
            .Skip((queryDto.Page - 1) * queryDto.PageSize)
            .Take(queryDto.PageSize)
            .Select(AddressMapper.ToListItem)
            .ToListAsync(ct);

        return new PagedResult<AddressListDto>(
            Items: items,
            Total: Math.Min(counted, AddressQueryDto.MaxResults),
            TotalCapped: counted > AddressQueryDto.MaxResults,
            Page: queryDto.Page,
            PageSize: queryDto.PageSize);
    }
    
    // Equality on Street uses the (Street, Id) index, so this stays fast on millions of rows.
    public Task<bool> Exists(AddressCreateDto dto, int? excludeId, CancellationToken ct) =>
        dbContext.Addresses.AnyAsync(a =>
            a.Id != excludeId &&
            a.Street == dto.Street &&
            a.Location.ZipCode == dto.ZipCode &&
            a.Location.Country.Name == dto.Country &&
            a.Recipient.Name == dto.Recipient, ct);

    public Task<Address?> GetById(int id, CancellationToken ct) =>
        dbContext.Addresses
            .AsNoTracking()
            .Include(a => a.Recipient)
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
    
    public async Task<bool> Delete(int id, CancellationToken ct)
    {
        var deletedCount = await dbContext.Addresses
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync<Address>(ct);

        return deletedCount > 0;
    }

    public Task<int> DeleteMany(int[] ids, CancellationToken ct) =>
        dbContext.Addresses.Where(a => ids.Contains(a.Id)).ExecuteDeleteAsync(ct);

    public async Task<IEnumerable<CountryDto>> GetCountries(CancellationToken ct)
    {
        var countries = await dbContext.Countries
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(CountryMapper.ToDto)
            .ToListAsync(ct);
        return countries;
    }
    
    public Task<Recipient?> FindRecipient(string name, CancellationToken ct) =>
        dbContext.Recipients.FirstOrDefaultAsync(r => r.Name == name, ct);

    public Task<Country?> FindCountry(string name, CancellationToken ct) =>
        dbContext.Countries.FirstOrDefaultAsync(c => c.Name == name, ct);

    public Task<Location?> FindLocation(int countryId, string zipCode, string name, CancellationToken ct) =>
        dbContext.Locations.FirstOrDefaultAsync(
            l => l.CountryId == countryId && l.ZipCode == zipCode && l.Name == name, ct);
}
