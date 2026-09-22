using AddressManagement.Application;
using AddressManagement.Application.Dtos;
using AddressManagement.Application.Mappers;
using AddressManagement.Domain;
using AddressManagement.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AddressManagement.Infrastructure.Repositories;

public class AddressRepository(AddressDbContext dbContext, ILogger<AddressRepository> logger)
    : IAddressRepository
{
    public async Task<IEnumerable<AddressListDto>> GetAll(CancellationToken ct)
    {
        try
        {
            var addresses = await dbContext.Addresses
                .Select(a => new AddressListDto(
                    a.Id,
                    a.Street,
                    a.Location.Name,
                    a.Location.ZipCode,
                    a.Location.Country.Name
                ))
                .ToListAsync(ct);
            
            return addresses;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all metadata");
            throw new Exception("Failed to retrieve address list", ex);
        }
    }

    public async void Add(AddressUpsertDto address, CancellationToken ct)
    {
                
    }
    // var addresses = new List<Address>
    // {
    //     new Address
    //     {
    //         Id = 1,
    //         Street = "Göpfritzgasse 6",
    //         Location = new Location
    //         {
    //             Id = 12,
    //             Name = "Wien",
    //             ZipCode = "1210",
    //             Country = new Country { Id = 123, Name = "Austria" }
    //         }
    //     }
    // };

}