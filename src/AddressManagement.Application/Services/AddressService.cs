using AddressManagement.Application.Dtos;
using AddressManagement.Application.Mappers;
using AddressManagement.Domain;

using AutoMapper;

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
        
        logger.LogInformation($"Address List: {addresses.First().Id}");
        return addresses;
    }

    public async Task<AddressDetailDto> Add(AddressUpsertDto addressDto, CancellationToken ct)
    {
        if (addressDto == null)
        {
            throw new InvalidDataException();
        }

        var savedAddressDto = addressRepository.Add(addressDto);
    }
}