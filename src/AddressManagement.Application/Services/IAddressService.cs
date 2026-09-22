using AddressManagement.Application.Dtos;

namespace AddressManagement.Application.Services;

public interface IAddressService
{
    Task<IEnumerable<AddressListDto>> GetAll(CancellationToken ct);
   
    Task<AddressDetailDto> Add(AddressUpsertDto addressDto, CancellationToken ct);
}