using System.Linq.Expressions;

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
        var addresses = dbContext.Addresses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(queryDto.Street))
        {
            addresses = addresses.Where(a => EF.Functions.ILike(a.Street, $"%{queryDto.Street}%"));
        }

        if (!string.IsNullOrWhiteSpace(queryDto.Location))
        {
            addresses = addresses.Where(a => EF.Functions.ILike(a.Location.Name, $"%{queryDto.Location}%"));
        }

        if (queryDto.Countries?.Length > 0)
        {
            addresses = addresses.Where(a => queryDto.Countries.Contains(a.Location.Country.Name));
        }

        Expression<Func<Address, string>> key = queryDto.SortCol switch
        {
            "zipCode" => a => a.Location.ZipCode,
            "location" => a => a.Location.Name,
            "country" => a => a.Location.Country.Name,
            "recipient" => a => a.Recipient.Name,
            _ => a => a.Street
        };
        
        var total = await addresses.CountAsync(ct);
        var orderedByCol = (queryDto.Desc ? addresses.OrderByDescending(key) : addresses.OrderBy(key)).ThenBy(a => a.Id);
        var items = await orderedByCol
            .Skip((queryDto.Page - 1) * queryDto.PageSize)
            .Take(queryDto.PageSize)
            .Select(AddressMapper.ToListItem)
            .ToListAsync(ct);

        return new PagedResult<AddressListDto>(items, total, queryDto.Page, queryDto.PageSize);
    }

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
