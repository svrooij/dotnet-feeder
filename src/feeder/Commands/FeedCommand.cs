using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using System.Net.Http.Json;
using System.Text;

namespace Feeder.Commands
{
    [Command("feed", Description = "Read a feed and write markdown")]
    public class FeedCommand : ICommand
    {
        [CommandParameter(0, Description = "Url of the feed to parse", IsRequired = true)]
        public string? Url { get; init; }

        [CommandParameter(1, Description = "File(s) to write posts to")]
        public IReadOnlyList<string>? Files { get; init; }

        [CommandOption("count", Description = "Number of items to use")]
        public int Count { get; init; } = 10;

        [CommandOption("tag", Description = "Tag to look for, <!-- start {tag} --> / <!-- end {tag} -->")]
        public string Tag { get; init; } = "posts";

        [CommandOption("template", Description = "Item template when writing markdown")]
        public string Template {get; init; } = "- [{title}]({url})";

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var cancellation = console.RegisterCancellationHandler();
            var feed = await LoadFeed(cancellation);
            console.Output.WriteLine("✅ Downloaded feed with {0} items", feed?.Items?.Length);
            await WriteToFiles(feed, console, cancellation);
        }

        private async ValueTask<Models.Feed> LoadFeed(CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(Url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new CommandException("Http exception", (int)response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<Models.Feed>(cancellationToken: cancellationToken);
        }

        private async Task WriteToFiles(Models.Feed feed, IConsole console, CancellationToken cancellationToken) {
            var sb = new StringBuilder();
            foreach(var item in feed.Items.Take(Count)) {
                sb.AppendLine(item.GetFormatted(Template));
            }

            var md = sb.ToString();

            foreach(var filename in Files) {
                await WriteToFile(md, filename, console, cancellationToken);
            }
        }

        private async Task WriteToFile(string newContent, string filename, IConsole console, CancellationToken cancellationToken) {
            var fixedFilename = Path.PathSeparator == '/' ? filename.Replace('\\', '/') : filename.Replace('/', '\\');
            var file = fixedFilename.StartsWith(".") ? Path.Combine(Directory.GetCurrentDirectory(), fixedFilename.Replace('/', '\\')) : fixedFilename;

            if(File.Exists(file)) {
                var content = await File.ReadAllTextAsync(file, Encoding.UTF8, cancellationToken);
                var startTag = $"<!-- start {Tag} -->";
                var endTag = $"<!-- end {Tag} -->";

                var firstContent = content.Split(startTag);
                if (firstContent.Length < 2)
                    return;
                
                var endContent = firstContent[1].Split(endTag);
                if (endContent.Length < 2) {
                    return;
                }

                await console.Output.WriteLineAsync($"✔ Modifying file {file}");

                await File.WriteAllTextAsync(file, $"{firstContent[0]}{startTag}\r\n{newContent}{endTag}{endContent[1]}");
            }
        }
    }
}