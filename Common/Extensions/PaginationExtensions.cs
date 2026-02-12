using AccountRegistrationApiDemo.DTOs.Responses;

namespace AccountRegistrationApiDemo.Common.Extensions;

/// <summary>
/// Extension methods for creating paginated responses from collections.
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Applies pagination to a queryable collection and returns a <see cref="PaginatedResponse{T}"/>.
    /// </summary>
    /// <param name="source">The full collection of items.</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <returns>A paginated response containing the current page of data plus metadata.</returns>
    public static PaginatedResponse<T> ToPaginatedResponse<T>(this IEnumerable<T> source, int page, int pageSize)
    {
        // Clamp to sane defaults
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;
        pageSize = pageSize > 100 ? 100 : pageSize;

        var items = source.ToList();
        var totalCount = items.Count;

        var data = items
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedResponse<T>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Data = data
        };
    }
}
