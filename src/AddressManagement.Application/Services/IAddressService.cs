using AddressManagement.Application.Dtos;

namespace AddressManagement.Application.Services;

public interface IAddressService
{
    Task<IEnumerable<AddressListItemDto>> GetAll();
}