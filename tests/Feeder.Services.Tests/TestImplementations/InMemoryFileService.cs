using Feeder.Base;

namespace Feeder.Services.Tests.TestImplementations
{
    internal class InMemoryFileService : IFileService
    {
        private readonly Dictionary<string, string> _files = new Dictionary<string, string>();

        public void DeleteFile(string filename)
        {
            _files.Remove(filename);
        }

        public bool FileExists(string filename)
        {
            return _files.ContainsKey(filename);
        }

        public ValueTask<string> ReadContentAsync(string filename, CancellationToken cancellationToken = default)
        {
            if (_files.TryGetValue(filename, out var content))
            {
                return new ValueTask<string>(content);
            }
            return default;
        }

        public string TempFilename()
        {
            var filename = Guid.NewGuid().ToString();
            _files[filename] = "";
            return filename;
        }

        public Task WriteContentAsync(string filename, string content, CancellationToken cancellationToken = default)
        {
            _files[filename] = content;
            return Task.CompletedTask;
        }
    }
}