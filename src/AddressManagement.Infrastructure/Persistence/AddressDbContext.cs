using AddressManagement.Application.Validators;
using AddressManagement.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressManagement.Infrastructure.Persistence;

public class AddressDbContext(DbContextOptions<AddressDbContext> options) : DbContext(options)
{
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Recipient> Recipients => Set<Recipient>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("addresses");
        b.HasPostgresExtension("pg_trgm");
        b.ApplyConfigurationsFromAssembly(typeof(AddressDbContext).Assembly);
        b.HasPostgresExtension("citext");
    }
}

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> b)
    {
        b.Property(a => a.Street).HasMaxLength(FieldLimits.Text);
        b.HasOne(a => a.Location).WithMany(l => l.Addresses)
            .HasForeignKey(a => a.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasOne(a => a.Recipient).WithMany(r => r.Addresses)
            .HasForeignKey(a => a.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(a => a.Street)
            .HasMethod("gin").HasOperators("gin_trgm_ops"); /* for efficient string pattern matching */
        b.HasIndex(a => new { a.Street, a.Id }); /* B-tree for the default ORDER BY (Street, Id) (GIN can't sort) */
    }
}

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> b)
    {
        b.Property(a => a.Name).HasColumnType("citext");
        b.Property(a => a.ZipCode).HasMaxLength(FieldLimits.Text);
        b.HasOne(a => a.Country).WithMany(l => l.Locations)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(a => new { a.CountryId, a.ZipCode, a.Name }).IsUnique(); // index used for find or create optimization (checked for exactly these columns) 
        b.HasIndex(a => a.ZipCode); // for Zipcode sorting
        b.HasIndex(a => a.Name); // for LocationName sorting
    }
}

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> b)
    {
        b.Property(c => c.Name).HasColumnType("citext");
        b.HasIndex(c => c.Name).IsUnique();
    }
}

public class RecipientConfiguration : IEntityTypeConfiguration<Recipient>
{
    public void Configure(EntityTypeBuilder<Recipient> b)
    {
        b.Property(c => c.Name).HasColumnType("citext");
        b.HasIndex(r => r.Name).IsUnique();
    }
}