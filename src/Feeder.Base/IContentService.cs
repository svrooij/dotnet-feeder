namespace Feeder.Base;

public interface IContentService
{
    public ValueTask<bool> ReplaceTextBetweenAsync(string filename, string startTag, string endTag, string substitution, CancellationToken cancellationToken = default);
}