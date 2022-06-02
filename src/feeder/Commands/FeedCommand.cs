using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Feeder.Base;
using Feeder.Base.Models;
using Feeder.Services;
using System.Text;

namespace Feeder.Commands;

[Command("feed", Description = "Read a feed and write markdown")]
public class FeedCommand : ICommand
{
    private readonly IFeedClient _feedClient;
    private readonly IContentService _contentService;

    public FeedCommand(IFeedClient feedClient, IContentService contentService)
    {
        _feedClient = feedClient;
        _contentService = contentService;
    }

    [CommandParameter(0, Description = "Url of the feed to parse", IsRequired = true)]
    public string? Url { get; init; }

    [CommandParameter(1, Description = "File(s) to write posts to")]
    public IReadOnlyList<string>? Files { get; init; }

    [CommandOption("count", Description = "Number of items to use")]
    public int Count { get; init; } = 10;

    [CommandOption("tag", Description = "Tag to look for, <!-- start {tag} --> / <!-- end {tag} -->")]
    public string Tag { get; init; } = "posts";

    [CommandOption("template", Description = "Item template when writing markdown")]
    public string Template { get; init; } = "- [{title}]({url})";

    [CommandOption("ci", Description = "Running in CI env, creates github logging", EnvironmentVariable = "CI")]
    public bool CI { get; init; } = false;

    [CommandOption("wordpress", Description = "Wordpress api has a different format")]
    public bool Wordpress {get; init;} = false;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        if (string.IsNullOrEmpty(Url)) return; // Can never happen is forced to be not null by CliFX
        var cancellationToken = console.RegisterCancellationHandler();
        var feed = await _feedClient.GetAsync(Url, new FeedOptions{ Wordpress = Wordpress }, cancellationToken);
        if(feed?.Items == null || feed.Items.Length == 0) {
            if (CI) {
                console.Output.WriteOutputVariable("files-updated", false);
            }
            return;
        }
        console.Output.WriteLine("✅ Downloaded feed with {0} items", feed?.Items?.Length);
        await WriteToFiles(feed, console, cancellationToken);
    }

    private async Task WriteToFiles(Feed? feed, IConsole console, CancellationToken cancellationToken)
    {
        if (Files == null || Files.Count == 0) return;
        if (feed?.Items == null) return;

        var sb = new StringBuilder();
        foreach (var item in feed.Items.Take(Count))
        {
            sb.AppendLine(item.FormatItem(Template));
        }

        var md = sb.ToString();

        var fileUpdated = false;

        foreach (var filename in Files)
        {
            fileUpdated = await WriteToFile(md, filename, console, cancellationToken) || fileUpdated;
        }

        if (CI) {
            console.Output.WriteOutputVariable("files-updated", fileUpdated);
        }
    }

    private async ValueTask<bool> WriteToFile(string newContent, string filename, IConsole console, CancellationToken cancellationToken)
    {
        var absolutePath = FileService.ConvertToAbsolute(filename);

        var result = await _contentService.ReplaceTextBetweenAsync(absolutePath, $"<!-- start {Tag} -->", $"<!-- end {Tag} -->", newContent, cancellationToken);

        await console.Output.WriteLineAsync($"{(result ? "✔" : "❌")} Modifying file {absolutePath}");
        return result;
    }
}