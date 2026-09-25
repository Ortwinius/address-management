using AddressManagement.Application;
using AddressManagement.Application.Dtos;
using AddressManagement.Application.Services;
using AddressManagement.Domain;

using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

namespace AddressManagement.Tests;

public class AddressServiceTests
{
    [Fact]
    public async Task Add_ReusesExistingCountryAndLocation()
    {
        var country = new Country { Id = 1, Name = "Austria" };
        var location = new Location { Id = 2, Name = "Wien", ZipCode = "1210", CountryId = 1, Country = country };

        var repository = Substitute.For<IAddressRepository>();
        repository.FindCountry("Austria", Arg.Any<CancellationToken>()).Returns(country);
        repository.FindLocation(1, "1210", "Wien", Arg.Any<CancellationToken>()).Returns(location);

        var service = new AddressService(NullLogger<AddressService>.Instance, repository);

        await service.Add(new AddressCreateDto("Teststraße 1", "1210", "Wien", "Austria", "Max Mustermann", null), CancellationToken.None);

        await repository.Received(1).Add(
            Arg.Is<Address>(a => a.Location == location && a.Location.Country == country),
            Arg.Any<CancellationToken>());
    }
}
