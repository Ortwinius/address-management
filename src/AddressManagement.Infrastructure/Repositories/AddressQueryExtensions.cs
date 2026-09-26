using System.Linq.Expressions;

using AddressManagement.Application.Dtos;
using AddressManagement.Domain;

using Microsoft.EntityFrameworkCore;

namespace AddressManagement.Infrastructure.Repositories;

internal static class AddressQueryExtensions
{
    public static IQueryable<Address> Filter(this IQueryable<Address> addresses, AddressQueryDto query)
    {
        if (!string.IsNullOrWhiteSpace(query.Street))
        {
            addresses = addresses.Where(a => EF.Functions.ILike(a.Street, $"%{query.Street}%"));
        }

        if (!string.IsNullOrWhiteSpace(query.Location))
        {
            addresses = addresses.Where(a => EF.Functions.ILike(a.Location.Name, $"%{query.Location}%"));
        }

        if (query.Countries?.Length > 0)
        {
            addresses = addresses.Where(a => query.Countries.Contains(a.Location.Country.Name));
        }

        return addresses;
    }

    public static IQueryable<Address> Sort(this IQueryable<Address> addresses, AddressQueryDto query)
    {
        Expression<Func<Address, string>> key = query.SortCol switch
        {
            AddressSortColumn.ZipCode => a => a.Location.ZipCode,
            AddressSortColumn.Location => a => a.Location.Name,
            AddressSortColumn.Country => a => a.Location.Country.Name,
            AddressSortColumn.Recipient => a => a.Recipient.Name,
            _ => a => a.Street
        };

        return query.Desc
            ? addresses.OrderByDescending(key).ThenByDescending(a => a.Id)
            : addresses.OrderBy(key).ThenBy(a => a.Id);
    }
}
