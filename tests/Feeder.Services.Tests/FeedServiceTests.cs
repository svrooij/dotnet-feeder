namespace Feeder.Services.Tests;

public class FeedServiceTests
{
    private readonly FeedClient _client;

    public FeedServiceTests()
    {
        _client = new FeedClient(new HttpClient());
    }

    [Theory()]
    [InlineData("https://svrooij.io/feed.json")]
    public async Task GetAsync_Result_NotEmtpy(string url)
    {
        var feed = await _client.GetAsync(url);

        Assert.NotNull(feed);
        Assert.NotNull(feed?.Title);
        Assert.NotNull(feed?.Items);
    }
}