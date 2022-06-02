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

    public async ValueTask<Feed?> GetAsync(string url, Base.FeedOptions options = null, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        if (options?.Wordpress == true) {
            var items = await response.Content.ReadFromJsonAsync<Models.WordpressItem[]>(cancellationToken: cancellationToken);
            return new Feed{
                Title = "Feed",
                Items = items?.Select(i => new Item {
                    Id = i.Id.ToString(),
                    Title = i.Title.Rendered,
                    Url = i.Link
                }).ToArray()
            };
        }

        return await response.Content.ReadFromJsonAsync<Feed>(cancellationToken: cancellationToken);
    }
}