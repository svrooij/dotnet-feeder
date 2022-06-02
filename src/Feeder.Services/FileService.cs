namespace Feeder.Services;

public class FileService : Base.IFileService
{
    public static string ConvertToAbsolute(string path)
    {
        var fixedFilename = Path.PathSeparator == '/' ? path.Replace('\\', '/') : path.Replace('/', '\\');
        return fixedFilename.StartsWith(".") ? Path.Combine(Directory.GetCurrentDirectory(), fixedFilename.Replace('/', '\\')) : fixedFilename;
    }

    public void DeleteFile(string filename)
    {
        if (File.Exists(filename))
        {
            File.Delete(filename);
        }
    }

    public bool FileExists(string filename)
    {
        return File.Exists(filename);
    }

    public async ValueTask<string> ReadContentAsync(string filename, CancellationToken cancellationToken = default)
    {
        return await File.ReadAllTextAsync(filename, cancellationToken);
    }

    public string TempFilename()
    {
        return Path.GetTempFileName();
    }

    public Task WriteContentAsync(string filename, string content, CancellationToken cancellationToken = default)
    {
        return File.WriteAllTextAsync(filename, content, cancellationToken);
    }
}