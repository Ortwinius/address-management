namespace AddressManagement.Application.Dtos;

public sealed record AddressListDto(
    int Id, 
    string Street, 
    string Location, 
    string Country);

public sealed record AddressDetailDto(
    int Id, 
    string Street, 
    string ZipCode, 
    string Location, 
    string Country,
    string? Recipient,
    string? AddressAffix
    );

public sealed record AddressCreateDto(
    string Street, 
    string ZipCode, 
    string Location, 
    string Country, 
    string? Recipient,
    string? AddressAffix
    );

public sealed record AddressSearchItemDto(int Id);

public sealed record PagedResult<T>(List<T> Items, int Total, int Page, int PageSize);

