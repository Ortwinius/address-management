using AddressManagement.Domain;

namespace AddressManagement.Infrastructure.Repositories;

public interface IAddressRepository
{
    Task<IEnumerable<Address>> GetAll();
}