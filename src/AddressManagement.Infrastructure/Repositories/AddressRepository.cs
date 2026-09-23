using AddressManagement.Application;
using AddressManagement.Application.Dtos;
using AddressManagement.Application.Mappers;
using AddressManagement.Domain;
using AddressManagement.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace AddressManagement.Infrastructure.Repositories;

public class AddressRepository(AddressDbContext dbContext) : IAddressRepository
{
    public Task<List<AddressListDto>> GetAll(CancellationToken ct) =>
        dbContext.Addresses
            .AsNoTracking()
            .Select(AddressMapper.ToListItem)
            .ToListAsync(ct);


    public Task<List<AddressDetailDto>> GetById(int id, CancellationToken ct) { throw new NotImplementedException();}
        // dbcontext.addresses
        //     .asnotracking()
        //     .firstordefaultasync(a => a.id == id, ct)
        //     .select(addressmapper.todetail);
    
    public Task<Country?> FindCountry(string name, CancellationToken ct) =>
        dbContext.Countries.FirstOrDefaultAsync(c => c.Name == name, ct);

    public Task<Location?> FindLocation(int countryId, string zipCode, string name, CancellationToken ct) =>
        dbContext.Locations.FirstOrDefaultAsync(
            l => l.CountryId == countryId && l.ZipCode == zipCode && l.Name == name, ct);

    public async Task Add(Address address, CancellationToken ct)
    {
        dbContext.Addresses.Add(address);
        await dbContext.SaveChangesAsync(ct);
    }
}
