using AddressManagement.Application.Dtos;
using AddressManagement.Application.Repositories;
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

        var addresses = Substitute.For<IAddressRepository>();
        var countries = Substitute.For<ICountryRepository>();
        var locations = Substitute.For<ILocationRepository>();
        countries.FindByName("Austria", Arg.Any<CancellationToken>()).Returns(country);
        locations.Find(1, "1210", "Wien", Arg.Any<CancellationToken>()).Returns(location);

        var service = new AddressService(NullLogger<AddressService>.Instance,
            addresses, Substitute.For<IRecipientRepository>(), countries, locations);

        await service.Add(new AddressCreateDto("Teststraße 1", "1210", "Wien", "Austria", "Max Mustermann", null), CancellationToken.None);

        await addresses.Received(1).Add(
            Arg.Is<Address>(a => a.Location == location && a.Location.Country == country),
            Arg.Any<CancellationToken>());
    }
}