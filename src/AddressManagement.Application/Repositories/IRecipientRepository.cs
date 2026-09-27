using AddressManagement.Domain;

namespace AddressManagement.Application.Repositories;

public interface IRecipientRepository
{
    Task<Recipient?> FindByName(string name, CancellationToken ct);
}