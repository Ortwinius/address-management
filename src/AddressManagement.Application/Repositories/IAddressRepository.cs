using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

namespace AddressManagement.Application.Repositories;

public interface IAddressRepository
{
    Task<PagedResult<AddressListDto>> GetAll(AddressQueryDto queryDto, CancellationToken ct);
    Task<Address?> GetById(int id, CancellationToken ct);
    Task Add(Address address, CancellationToken ct);
    Task Update(Address address, CancellationToken ct);
    Task<bool> Exists(AddressCreateDto dto, int? excludeId, CancellationToken ct);
    Task<bool> Delete(int id, CancellationToken ct);
    Task<int> DeleteMany(int[] ids, CancellationToken ct);
}
