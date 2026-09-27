using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

namespace AddressManagement.Application;

public interface IAddressRepository
{
    Task<IEnumerable<AddressListDto>> GetAll(CancellationToken ct);
    Task<AddressDetailDto> Add(AddressUpsertDto address, CancellationToken ct);
    Task<AddressDetailDto> GetById(string Id, CancellationToken ct);
}