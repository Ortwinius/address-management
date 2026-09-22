using AddressManagement.Application.Dtos;
using AddressManagement.Infrastructure.Repositories;

using Microsoft.Extensions.Logging;

namespace AddressManagement.Application.Services;

public class AddressService(
    ILogger<AddressService> logger,
    IAddressRepository addressRepository
    ) : IAddressService
{
    public async Task<IEnumerable<AddressListItemDto>> GetAll()
    {
        throw new NotImplementedException();
        var addresses = await addressRepository.GetAll();
        // // using Mapper in Service instead of ProjectTo MetadataListDto in Repo to keep separation of concerns
        // var allListMetadata = _mapper.Map<IEnumerable<DocumentListDto>>(allMetadata);
        // return allListMetadata;
    } 
}