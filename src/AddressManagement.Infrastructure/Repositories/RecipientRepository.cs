using AddressManagement.Application.Repositories;
using AddressManagement.Domain;
using AddressManagement.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace AddressManagement.Infrastructure.Repositories;

public class RecipientRepository(AddressDbContext dbContext) : IRecipientRepository
{
    public Task<Recipient?> FindByName(string name, CancellationToken ct) =>
        dbContext.Recipients.FirstOrDefaultAsync(r => r.Name == name, ct);
}