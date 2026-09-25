using AddressManagement.Domain;

using Bogus;

using Microsoft.EntityFrameworkCore;

namespace AddressManagement.Infrastructure.Persistence;

// Dev only: fills the DB with fake addresses to test search and paging on large data.
public static class DevDataSeeder
{
    private const int LocationCount = 1_000;
    private const int RecipientCount = 1_000;
    private const int BatchSize = 10_000; // keeps EF change tracking small

    // Tops the address table up to targetCount.
    public static async Task Seed(AddressDbContext db, int targetCount)
    {
        var missing = targetCount - await db.Addresses.CountAsync();
        if (missing <= 0) return;

        // Reuse countries that already exist (name is unique).
        string[] names = ["Österreich", "Deutschland", "Schweiz", "Luxemburg", "Liechtenstein"];
        var countries = await db.Countries.Where(c => names.Contains(c.Name)).ToListAsync();
        countries.AddRange(names.Except(countries.Select(c => c.Name)).Select(n => new Country { Name = n }));

        var locations = new Faker<Location>("de")
            .RuleFor(l => l.Name, f => f.Address.City())
            .RuleFor(l => l.ZipCode, f => f.Address.ZipCode())
            .RuleFor(l => l.Country, f => f.PickRandom(countries))
            .Generate(LocationCount)
            .DistinctBy(l => (l.Country.Name, l.ZipCode, l.Name)) // (country, zip, name) is unique
            .ToList();
        db.Locations.AddRange(locations);
        await db.SaveChangesAsync();
        var locationIds = locations.Select(l => l.Id).ToArray();

        var addresses = new Faker<Address>("de")
            .RuleFor(a => a.Street, f => f.Address.StreetAddress())
            .RuleFor(a => a.LocationId, f => f.PickRandom(locationIds));

        for (var seeded = 0; seeded < missing; seeded += BatchSize)
        {
            db.ChangeTracker.Clear();
            db.Addresses.AddRange(addresses.Generate(Math.Min(BatchSize, missing - seeded)));
            await db.SaveChangesAsync();
        }
    }
}
