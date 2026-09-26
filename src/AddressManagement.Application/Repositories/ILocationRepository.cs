using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

namespace AddressManagement.Application.Repositories;

public interface ILocationRepository
{
    Task<Location?> Find(int countryId, string zipCode, string name, CancellationToken ct);
}