namespace MovieAPI.Core.Utils;

public class QueryParameterFactory
{
    public static Dictionary<string, string> CreateQueryParameterByName(string name, int page)
    {
        return new Dictionary<string, string>
        {
            ["query"] = name,
            ["include_adult"] = false.ToString(),
            ["language"] = "en-US",
            ["page"] = page.ToString(),
        };
    }
}
