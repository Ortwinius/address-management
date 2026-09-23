using AddressManagement.Application.Dtos;
using AddressManagement.Application.Mappers;
using AddressManagement.Domain;

using Microsoft.Extensions.Logging;

namespace AddressManagement.Application.Services;

public class AddressService(
    ILogger<AddressService> logger,
    IAddressRepository addressRepository
    ) : IAddressService
{
    public async Task<IEnumerable<AddressListDto>> GetAll(CancellationToken ct)
    {
        var addresses = await addressRepository.GetAll(ct);

        logger.LogInformation("Loaded {Count} addresses", addresses.Count);
        return addresses;
    }


    public Task<IEnumerable<AddressDetailDto>> GetById(int id, CancellationToken ct)
    {
        var address = await addressRepository.GetById(id);
        if (address == null)
        {
            // exception? wenn ja was für eine ?
        }

        return address;
    }
    public async Task<AddressDetailDto> Add(AddressCreateDto dto, CancellationToken ct)
    {
        // Find-or-create keeps Country/Location normalized (shared by many addresses).
        var country = await addressRepository.FindCountry(dto.Country, ct)
                      ?? new Country { Name = dto.Country };

        var location = (country.Id != 0
                           ? await addressRepository.FindLocation(country.Id, dto.ZipCode, dto.Location, ct)
                           : null)
                       ?? new Location { Name = dto.Location, ZipCode = dto.ZipCode, Country = country };

        var address = new Address
        {
            Street = dto.Street,
            Recipient = dto.Recipient,
            Location = location
        };

        await addressRepository.Add(address, ct);

        logger.LogInformation("Created address {AddressId}", address.Id);
        return AddressMapper.ToDetail(address);
    }
}
