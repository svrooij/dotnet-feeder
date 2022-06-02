using Feeder.Base;

namespace Feeder.Services.Tests;

public class FileServiceTests : IDisposable
{
    private readonly IFileService fileService;
    private readonly string filename;

    public FileServiceTests()
    {
        this.fileService = new FileService();
        filename = fileService.TempFilename();
    }

    [Fact]
    public void DeleteFile_Works()
    {
        var fileExists = fileService.FileExists(filename);
        Assert.True(fileExists);

        fileService.DeleteFile(filename);

        var fileExistsAfterDelete = fileService.FileExists(filename);
        Assert.False(fileExistsAfterDelete);
    }

    [Fact]
    public async Task ReadAndWriteContent_Works()
    {
        var content = Guid.NewGuid().ToString();

        var currentContent = await fileService.ReadContentAsync(filename);
        Assert.Equal("", currentContent);

        await fileService.WriteContentAsync(filename, content);

        var newContent = await fileService.ReadContentAsync(filename);
        Assert.Equal(content, newContent);
    }

    public void Dispose()
    {
        fileService.DeleteFile(filename);
    }
}