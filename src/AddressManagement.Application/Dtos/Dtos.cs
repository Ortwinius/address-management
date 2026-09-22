namespace AddressManagement.Application.Dtos;

public sealed record AddressListItemDto(int AddressId, string Street, string ZipCode);

public sealed record AddressDetailItemDto(
    int AddressId, 
    string Street, 
    string ZipCode, 
    string Location, 
    string Country,
    string? Recipient
    );

public sealed record AddressUpsertDto(
    string Street, 
    string ZipCode, 
    string Location, 
    string Country, 
    string? Recipient
    );

public sealed record AddressSearchItemDto(int Id);

