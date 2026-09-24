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

public sealed record CountryDto(string Name);

public sealed record AddressQueryDto(
    string? Street,
    string? Location,
    string[]? Countries,
    string SortCol = "Street",
    bool Desc = false,
    int Page = 1,
    int PageSize = 10
);

public sealed record PagedResult<T>(List<T> Items, int Total, int Page, int PageSize);

