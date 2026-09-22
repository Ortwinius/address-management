using AddressManagement.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressManagement.Infrastructure.DbContext;

public class AddressDbContext(DbContextOptions<AddressDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Country> Countries => Set<Country>();
    
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("addresses");
        b.HasPostgresExtension("pg_trgm");
        b.ApplyConfigurationsFromAssembly(typeof(AddressDbContext).Assembly);
    }
}

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> b)
    {
        b.Property(a => a.Street).HasMaxLength(200);
        b.HasOne(a => a.Location).WithMany(l => l.Addresses)
            .HasForeignKey(a => a.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(a => a.Street);
        // .HasMethod("gin").HasOperators("gin_trgm_ops");
    }
}

public class LocationConfigration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> b)
    {
        b.Property(a => a.Name).HasMaxLength(200);
        b.Property(a => a.ZipCode).HasMaxLength(20);
        b.HasOne(a => a.Country).WithMany(l => l.Locations)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(a => new {a.CountryId, a.ZipCode, a.Name}).IsUnique();
        // .HasMethod("gin").HasOperators("gin_trgm_ops");
    }
}
