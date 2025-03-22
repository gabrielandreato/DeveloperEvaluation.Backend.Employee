using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ModelLibrary.Common;

/// <summary>
/// Represents a paginated list of items.
/// Provides metadata about the pagination such as page count and total count of items.
/// </summary>
/// <typeparam name="T">The type of the elements in the list.</typeparam>
public class PagedList<T>
{
    /// <summary>
    /// Gets or sets the paginated items.
    /// </summary>
    public List<T> Items { get; set; }

    /// <summary>
    /// Gets or sets the current page index (zero-based).
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the size of each page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total count of items available.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets a value indicating whether there is a next page available.
    /// </summary>
    public bool HasNextPage => Page * PageSize < TotalCount;

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// Asynchronously creates a paginated and sorted list from a queryable data source.
    /// </summary>
    /// <param name="query">The queryable data source to paginate and sort.</param>
    /// <param name="page">The zero-based page index to retrieve. Defaults to 0 if not provided.</param>
    /// <param name="pageSize">The number of items per page. Defaults to 0, retrieving all items if not provided.</param>
    /// <param name="sortBy">The property name to sort the items by. Sort is skipped if null or empty.</param>
    /// <param name="isDesc">Determines the sorting direction. True for descending order, false for ascending. Defaults to false.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to CancellationToken.None.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a <see cref="PagedList{T}"/> with the sorted and paginated data.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the property specified in <paramref name="sortBy"/> does not exist.</exception>
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page = 0, int pageSize = 0,
        string? sortBy = null, bool isDesc = false, CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrEmpty(sortBy))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = isDesc ? "OrderByDescending" : "OrderBy";
            var orderByExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                query.Expression,
                Expression.Quote(lambda)
            );

            query = query.Provider.CreateQuery<T>(orderByExpression);
        }

        List<T> items;
        if (page != 0 && pageSize != 0)
            items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        else
            items = await query.ToListAsync(cancellationToken);

        return new PagedList<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}