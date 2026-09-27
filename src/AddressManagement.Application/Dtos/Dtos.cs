namespace AddressManagement.Application.Dtos;

public sealed record AddressListDto(
    int Id,
    string Street,
    string ZipCode,
    string Location,
    string Country,
    string Recipient);

public sealed record AddressDetailDto(
    int Id,
    string Street,
    string ZipCode,
    string Location,
    string Country,
    string Recipient,
    string? AddressAffix
);

public sealed record AddressCreateDto(
    string Street,
    string ZipCode,
    string Location,
    string Country,
    string Recipient,
    string? AddressAffix
);

public sealed record CountryDto(string Name);

// No Country: sorting by it can't use an index.
public enum AddressSortColumn { Street, ZipCode, Location, Recipient }

public sealed record AddressQueryDto(
    string? Street,
    string? Location,
    string[]? Countries,
    AddressSortColumn SortCol = AddressSortColumn.Street,
    bool Desc = false,
    int Page = 1,
    int PageSize = 10
)
{
    public const int MaxResults = 100_000;
    public const int MinStreetSearchLength = 3; // the trigram index on Street needs 3 characters
}

public sealed record PagedResult<T>(IEnumerable<T> Items, int Total, bool TotalCapped, int Page, int PageSize);