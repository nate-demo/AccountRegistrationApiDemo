namespace AccountRegistrationApiDemo.DTOs.Responses;

/// <summary>
/// Generic wrapper for paginated API responses.
/// </summary>
/// <typeparam name="T">The type of items in the current page.</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>Current page number (1-based).</summary>
    public int Page { get; set; }

    /// <summary>Number of items per page.</summary>
    public int PageSize { get; set; }

    /// <summary>Total number of items across all pages.</summary>
    public int TotalCount { get; set; }

    /// <summary>Total number of pages.</summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

    /// <summary>The items on the current page.</summary>
    public List<T> Data { get; set; } = [];
}
