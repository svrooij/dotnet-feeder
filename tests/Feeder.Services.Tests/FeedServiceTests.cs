namespace Feeder.Services.Tests;

public class FeedServiceTests
{
    private readonly FeedClient _client;

    public FeedServiceTests()
    {
        _client = new FeedClient(new HttpClient());
    }

    [Theory()]
    [InlineData("https://svrooij.io/feed.json", false)]
    [InlineData("https://reizen.inbalansopweg.nl/wp-json/wp/v2/posts?_fields=id,title,link&per_page=2", true)]
    public async Task GetAsync_Result_NotEmtpy(string url, bool wordpress)
    {
        var feed = await _client.GetAsync(url, new Base.FeedOptions{ Wordpress = wordpress });

        Assert.NotNull(feed);
        Assert.NotNull(feed?.Title);
        Assert.NotNull(feed?.Items);
    }
}