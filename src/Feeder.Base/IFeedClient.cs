namespace Feeder.Base;

public interface IFeedClient
{
    public ValueTask<Models.Feed?> GetAsync(string url, CancellationToken cancellationToken = default);
}