using AddressManagement.Application.Dtos;
using AddressManagement.Application.Exceptions;
using AddressManagement.Application.Mappers;
using AddressManagement.Application.Repositories;
using AddressManagement.Domain;

using Microsoft.Extensions.Logging;

namespace AddressManagement.Application.Services;

public class AddressService(
    ILogger<AddressService> logger,
    IAddressRepository addressRepository,
    IRecipientRepository recipientRepository,
    ICountryRepository countryRepository,
    ILocationRepository locationRepository
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
        dto = Trim(dto);
        await EnsureNotDuplicate(dto, id: null, ct);

        var address = new Address
        {
            Street = dto.Street,
            Recipient = await ResolveRecipient(dto.Recipient, ct),
            AddressAffix = dto.AddressAffix,
            Location = await ResolveLocation(dto, ct)
        };

        await addressRepository.Add(address, ct);

        logger.LogInformation("Created address {AddressId}", address.Id);
        return AddressMapper.ToDetail(address);
    }

    public async Task<AddressDetailDto?> Update(int id, AddressCreateDto dto, CancellationToken ct)
    {
        var address = await addressRepository.GetById(id, ct);
        if (address is null)
        {
            return null;
        }

        dto = Trim(dto);
        await EnsureNotDuplicate(dto, id: id, ct);

        address.Street = dto.Street;
        address.Recipient = await ResolveRecipient(dto.Recipient, ct);
        address.AddressAffix = dto.AddressAffix;
        address.Location = await ResolveLocation(dto, ct);

        await addressRepository.Update(address, ct);

        logger.LogInformation("Updated address {AddressId}", address.Id);
        return AddressMapper.ToDetail(address);
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
        await recipientRepository.FindByName(name, ct) ?? new Recipient { Name = name };

    // Find-or-create keeps Country/Location normalized (shared by many addresses).
    private async Task<Location> ResolveLocation(AddressCreateDto dto, CancellationToken ct)
    {
        var country = await countryRepository.FindByName(dto.Country, ct)
                      ?? new Country { Name = dto.Country };

        return (country.Id != 0
                   ? await locationRepository.Find(country.Id, dto.ZipCode, dto.Location, ct)
                   : null)
               ?? new Location { Name = dto.Location, ZipCode = dto.ZipCode, Country = country };
    }

    private async Task EnsureNotDuplicate(AddressCreateDto dto, int? id, CancellationToken ct)
    {
        if (await addressRepository.Exists(dto, id, ct))
        {
            throw new ConflictException("Address already exists.");
        }
    }

    // " Austria" must find "Austria". Upper/lower case is handled by the citext columns.
    private static AddressCreateDto Trim(AddressCreateDto dto) => dto with
    {
        Street = dto.Street.Trim(),
        ZipCode = dto.ZipCode.Trim(),
        Location = dto.Location.Trim(),
        Country = dto.Country.Trim(),
        Recipient = dto.Recipient.Trim(),
        AddressAffix = string.IsNullOrWhiteSpace(dto.AddressAffix) ? null : dto.AddressAffix.Trim()
    };
}