namespace AddressManagement.Application.Dtos;

public sealed record AddressListDto(
    int Id, 
    string Street, 
    string Location, 
    // string ZipCode, 
    string Country);

public sealed record AddressDetailDto(
    int Id, 
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

