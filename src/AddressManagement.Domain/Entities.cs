using System.ComponentModel.DataAnnotations;

namespace AddressManagement.Domain;

public class Address
{
    [Key] public int Id { get; set; }
    public required string Street { get; set; } //HouseNumber?
    public string? Recipient { get; set; }
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