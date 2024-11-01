namespace MovieAPI.Core.Models;

public class Response<T> where T : MediaBase
{
    public int Page { get; set; } = 1;
    public IEnumerable<T> Results { get; set; } = Enumerable.Empty<T>();

    public int TotalPages { get; set; }

    public int TotalResults { get; set; }
}