using System.ComponentModel.DataAnnotations;

namespace AddressManagement.Domain;

// public static const int MaxVarCharLength = 255;
public class Address
{
    [Key] public int Id { get; set; }
    [MaxLength(200)] public required string Street { get; set; } /* e.g. "Neubaugasse 2-8", includes house number etc. */
    public string? Recipient { get; set; }
    public string? AddressAffix { get; set; } /* "Addresszusatz" */
    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;
}

public class Location
{
   [Key] public int Id { get; set; }
   public required string Name { get; set; }
   public required string ZipCode { get; set; } 
   public int CountryId { get; set; }
   public Country Country { get; set; } = null!;
   public ICollection<Address> Addresses { get; set; } = [];
}

public class Country
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Location> Locations { get; set; } = [];
}

public class Recipient
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Address> Addresses { get; set; } = [];
}