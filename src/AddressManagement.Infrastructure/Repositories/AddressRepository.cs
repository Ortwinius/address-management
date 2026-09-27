using AddressManagement.Application.Dtos;
using AddressManagement.Application.Mappers;
using AddressManagement.Application.Repositories;
using AddressManagement.Domain;
using AddressManagement.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace AddressManagement.Infrastructure.Repositories;

public class AddressRepository(AddressDbContext dbContext) : IAddressRepository
{
    // Up to this many matches, loading and sorting just their ids takes milliseconds.
    private const int SmallResult = 10_000;

    public async Task<PagedResult<AddressListDto>> GetAll(AddressQueryDto queryDto, CancellationToken ct)
    {
        var addresses = dbContext.Addresses.AsNoTracking().FilterStreet(queryDto);

        // Location and country filters only need the small Locations table. Resolving them to ids first gives
        // Postgres exact values to plan with, instead of a join estimate that was off by orders of magnitude.
        if (!string.IsNullOrWhiteSpace(queryDto.Location) || queryDto.Countries?.Length > 0)
        {
            var locationIds = await dbContext.Locations.Filter(queryDto).Select(l => l.Id).ToListAsync(ct);
            addresses = addresses.Where(a => locationIds.Contains(a.LocationId));
        }

        // Counting millions of rows is expensive, so stop at MaxResults (+1 tells whether there are more).
        var counted = await addresses.Take(AddressQueryDto.MaxResults + 1).CountAsync(ct);

        // For ORDER BY + LIMIT, Postgres may walk a sort index until enough rows match. With only a few matches
        // among millions of rows that walk takes seconds, so small results load their ids and sort only those.
        if (counted <= SmallResult)
        {
            var ids = await addresses.Select(a => a.Id).ToListAsync(ct);
            addresses = dbContext.Addresses.AsNoTracking().Where(a => ids.Contains(a.Id));
        }

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
}
