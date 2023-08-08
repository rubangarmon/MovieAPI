using System.Collections.Specialized;
using System.Web;

namespace MovieAPI.Infrastructure.Extensions
{
    public static class DictionaryExtension
    {
        public static NameValueCollection ConvertToQueryParameters(this Dictionary<string, string?> dictionaryParameters)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            foreach (var pair in dictionaryParameters)
            {
                parameters.Add(pair.Key, pair.Value);
            }

            return parameters;
        }
    }
}
