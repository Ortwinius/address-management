using AddressManagement.Application.Dtos;

namespace AddressManagement.Application.Services;

public interface IAddressService
{
    Task<IEnumerable<AddressListDto>> GetAll(CancellationToken ct);
    
    Task<IEnumerable<AddressDetailDto>> GetById(int id, CancellationToken ct);
   
    Task<AddressDetailDto> Add(AddressCreateDto addressDto, CancellationToken ct);
}