using AddressManagement.Domain;
using AddressManagement.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AddressManagement.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private AddressDbContext _dbContext;
    private ILogger<AddressRepository> _logger;

    public AddressRepository(AddressDbContext dbContext, ILogger<AddressRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Address>> GetAll()
    {
        try
        {
            return await _dbContext.Addresses
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all metadata");
            // throw new Exception("Failed to retrieve metadata list", ex);
        }
        throw new NotImplementedException();
    }
}