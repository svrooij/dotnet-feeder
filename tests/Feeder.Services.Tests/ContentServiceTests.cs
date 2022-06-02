using Feeder.Base;

namespace Feeder.Services.Tests
{
    public class ContentServiceTests : IDisposable
    {
        private static string StartTag(string tag = "posts") => $"<!-- start {tag} -->";

        private static string EndTag(string tag = "posts") => $"<!-- end {tag} -->";

        private static string GetTestContent(string tag = "posts", string content = "") =>
            $"Test\r\n{StartTag(tag)}\r\n{content}{EndTag(tag)}\r\nMore info";

        private readonly IContentService _contentService;
        private readonly IFileService _fileService;
        private readonly string filename;

        public ContentServiceTests()
        {
            _fileService = new TestImplementations.InMemoryFileService();
            _contentService = new ContentService(_fileService);
            filename = _fileService.TempFilename();
        }

        [Fact]
        public async Task ReplaceText_NewContent_ContainsSubstitution()
        {
            var testContent = GetTestContent();
            await _fileService.WriteContentAsync(filename, testContent);

            var substitution = "Fancy content\r\nwith new lines.";

            var result = await _contentService.ReplaceTextBetweenAsync(filename, StartTag(), EndTag(), substitution);

            Assert.True(result);

            var content = await _fileService.ReadContentAsync(filename);
            Assert.NotEqual(testContent, content);

            var index = content.IndexOf(substitution);
            Assert.Equal(28, index);
        }

        [Fact]
        public async Task ReplaceText_NewContent_ReplacedExistingText()
        {
            var textToRemove = "TEXT TO REMOVE!!";
            var testContent = GetTestContent(content: textToRemove);
            await _fileService.WriteContentAsync(filename, testContent);

            var substitution = "Fancy content\r\nwith new lines.";

            var result = await _contentService.ReplaceTextBetweenAsync(filename, StartTag(), EndTag(), substitution);

            Assert.True(result);

            var content = await _fileService.ReadContentAsync(filename);
            Assert.NotEqual(testContent, content);

            var index = content.IndexOf(textToRemove);
            Assert.Equal(-1, index);
        }

        public void Dispose()
        {
            _fileService.DeleteFile(filename);
        }
    }
}