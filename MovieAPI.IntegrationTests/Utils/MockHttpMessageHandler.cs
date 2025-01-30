using System.Net;

namespace MovieAPI.IntegrationTests.Utils;
public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Dictionary<string, HttpResponseMessage> _responses = new();

    public void RegisterResponse(string url, string filePath, HttpStatusCode statusCode)
    {
        var jsonResponse = File.ReadAllText(filePath);
        var content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json");
        var response = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = content
        };
        _responses[url] = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request?.RequestUri?.PathAndQuery != null && _responses.TryGetValue(request.RequestUri.PathAndQuery, out var response))
        {
            return Task.FromResult(response);
        }
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
    }
}
