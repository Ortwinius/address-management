using System.Linq.Expressions;

using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

using Microsoft.EntityFrameworkCore;

namespace AddressManagement.Infrastructure.Repositories;

internal static class AddressQueryExtensions
{
    private const string LikeEscape = @"\";

    public static IQueryable<Address> FilterStreet(this IQueryable<Address> addresses, AddressQueryDto query)
    {
        if (!string.IsNullOrWhiteSpace(query.Street))
        {
            var pattern = ContainsPattern(query.Street);
            addresses = addresses.Where(a => EF.Functions.ILike(a.Street, pattern, LikeEscape));
        }

        return addresses;
    }

    public static IQueryable<Location> Filter(this IQueryable<Location> locations, AddressQueryDto query)
    {
        if (!string.IsNullOrWhiteSpace(query.Location))
        {
            var pattern = ContainsPattern(query.Location);
            locations = locations.Where(l => EF.Functions.ILike(l.Name, pattern, LikeEscape));
        }

        if (query.Countries?.Length > 0)
        {
            locations = locations.Where(l => query.Countries.Contains(l.Country.Name));
        }

        return locations;
    }

    // "%" and "_" are LIKE wildcards. Escaped, they are searched for literally and can't bypass the trigram index.
    private static string ContainsPattern(string term) =>
        "%" + term.Trim()
            .Replace(LikeEscape, LikeEscape + LikeEscape)
            .Replace("%", LikeEscape + "%")
            .Replace("_", LikeEscape + "_") + "%";

    public static IQueryable<Address> Sort(this IQueryable<Address> addresses, AddressQueryDto query)
    {
        Expression<Func<Address, string>> key = query.SortCol switch
        {
            AddressSortColumn.ZipCode => a => a.Location.ZipCode,
            AddressSortColumn.Location => a => a.Location.Name,
            AddressSortColumn.Recipient => a => a.Recipient.Name,
            _ => a => a.Street
        };

        return query.Desc
            ? addresses.OrderByDescending(key).ThenByDescending(a => a.Id)
            : addresses.OrderBy(key).ThenBy(a => a.Id);
    }
}
