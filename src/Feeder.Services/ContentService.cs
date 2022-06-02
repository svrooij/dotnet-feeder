using Feeder.Base;

namespace Feeder.Services;

public class ContentService : IContentService
{
    private readonly IFileService _fileService;

    public ContentService(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async ValueTask<bool> ReplaceTextBetweenAsync(string filename, string startTag, string endTag, string substitution, CancellationToken cancellationToken = default)
    {
        if (_fileService.FileExists(filename))
        {
            var currentContent = await _fileService.ReadContentAsync(filename, cancellationToken);

            var firstContent = currentContent.Split(startTag);
            if (firstContent.Length < 2) { return false; }

            var lastContent = firstContent[1].Split(endTag);
            if (lastContent.Length < 2) { return false; }

            var newContent = firstContent[0] + startTag + "\r\n" + substitution + endTag + lastContent[1];

            await _fileService.WriteContentAsync(filename, newContent, cancellationToken);
            return true;
        }

        return false;
    }
}