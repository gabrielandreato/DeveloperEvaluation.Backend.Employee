namespace ModelLibrary.Common;

public class DefaultQueryParameter
{
    /// <summary>
    /// The page number for pagination (default is 0, which returns all results).
    /// </summary>
    public int? Page { get; set; } = 0;
    
    /// <summary>
    /// The number of items per page (default is 0, which returns all results).
    /// </summary>
    public int? PageSize { get; set; } = 0;
    
    /// <summary>
    /// Optional field name to sort results by. If null, no sorting is applied.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Indicates whether sorting should be descending (true) or ascending (false). Default is false.
    /// </summary>
    public bool? IsDesc { get; set; } = false;

}