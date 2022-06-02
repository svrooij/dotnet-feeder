namespace Feeder.Base;

public interface IFileService
{
    public void DeleteFile(string filename);

    public bool FileExists(string filename);

    public string TempFilename();

    public ValueTask<string> ReadContentAsync(string filename, CancellationToken cancellationToken = default);

    public Task WriteContentAsync(string filename, string content, CancellationToken cancellationToken = default);
}