using AddressManagement.Application.Dtos;
using AddressManagement.Application.Mappers;
using AddressManagement.Domain;

using Microsoft.Extensions.Logging;

using static AddressManagement.Application.Mappers.AddressMapper;

namespace AddressManagement.Application.Services;

public class AddressService(
    ILogger<AddressService> logger,
    IAddressRepository addressRepository
    ) : IAddressService
{
    public async Task<PagedResult<AddressListDto>> GetAll(AddressQueryDto queryDto, CancellationToken ct)
    {
        var result = await addressRepository.GetAll(queryDto, ct);

        logger.LogInformation("Loaded {Count} of {Total} addresses", result.Items.Count, result.Total);
        return result;
    }

    public async Task<AddressDetailDto?> GetById(int id, CancellationToken ct)
    {
        var address = await addressRepository.GetById(id, ct);
        return address is null ? null : AddressMapper.ToDetail(address);
    }

    public async Task<AddressDetailDto> Add(AddressCreateDto dto, CancellationToken ct)
    {
        var address = new Address
        {
            Street = dto.Street,
            Recipient = await ResolveRecipient(dto.Recipient, ct),
            AddressAffix = dto.AddressAffix,
            Location = await ResolveLocation(dto, ct)
        };

        await addressRepository.Add(address, ct);

        logger.LogInformation("Created address {AddressId}", address.Id);
        return ToDetail(address);
    }

    public async Task<AddressDetailDto?> Update(int id, AddressCreateDto dto, CancellationToken ct)
    {
        var address = await addressRepository.GetById(id, ct);
        if (address is null)
        {
            return null;
        }

        address.Street = dto.Street;
        address.Recipient = await ResolveRecipient(dto.Recipient, ct);
        address.AddressAffix = dto.AddressAffix;
        // Re-point to a (possibly new) Location instead of editing the shared one.
        address.Location = await ResolveLocation(dto, ct);

        await addressRepository.Update(address, ct);

        logger.LogInformation("Updated address {AddressId}", address.Id);
        return ToDetail(address);
    }

    public async Task<bool> Delete(int id, CancellationToken ct)
    {
        return await addressRepository.Delete(id, ct);
    }

    public async Task<int> DeleteMany(int[] ids, CancellationToken ct)
    {
        var deleted = await addressRepository.DeleteMany(ids, ct);
        logger.LogInformation("Deleted {Count} addresses", deleted);
        return deleted;
    }

    // Find-or-create, so one person or company is stored once and shared by its addresses.
    private async Task<Recipient> ResolveRecipient(string name, CancellationToken ct) =>
        await addressRepository.FindRecipient(name, ct) ?? new Recipient { Name = name };
    // Find-or-create keeps Country/Location normalized (shared by many addresses).
    private async Task<Location> ResolveLocation(AddressCreateDto dto, CancellationToken ct)
    {
        var country = await addressRepository.FindCountry(dto.Country, ct)
                      ?? new Country { Name = dto.Country };

        return (country.Id != 0
                   ? await addressRepository.FindLocation(country.Id, dto.ZipCode, dto.Location, ct)
                   : null)
               ?? new Location { Name = dto.Location, ZipCode = dto.ZipCode, Country = country };
    }

    public async Task<IEnumerable<CountryDto>> GetCountries(CancellationToken ct)
    {
        var countries = await addressRepository.GetCountries(ct);
        return countries;
    }
}
