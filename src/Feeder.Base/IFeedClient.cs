namespace Feeder.Base;

public class FeedOptions {
    public bool Wordpress { get; set; }
}
public interface IFeedClient
{
    public ValueTask<Models.Feed?> GetAsync(string url, FeedOptions? options = null, CancellationToken cancellationToken = default);
}