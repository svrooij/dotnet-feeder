using Feeder.Base.Models;
using System.Net.Http.Json;

namespace Feeder.Services;

public class FeedClient : Base.IFeedClient
{
    private readonly HttpClient _httpClient;

    public FeedClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async ValueTask<Feed?> GetAsync(string url, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Feed>(cancellationToken: cancellationToken);
    }
}