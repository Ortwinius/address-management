using AddressManagement.Application.Dtos;

namespace AddressManagement.Application.Services;

public interface IAddressService
{
    Task<PagedResult<AddressListDto>> GetAll(AddressQueryDto queryDto, CancellationToken ct);

    Task<AddressDetailDto?> GetById(int id, CancellationToken ct);

    Task<AddressDetailDto> Add(AddressCreateDto addressDto, CancellationToken ct);

    Task<AddressDetailDto?> Update(int id, AddressCreateDto addressDto, CancellationToken ct);

    Task<bool> Delete(int id, CancellationToken ct);
}
