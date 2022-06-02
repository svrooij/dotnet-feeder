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

    public async ValueTask ExecuteAsync(IConsole console)
    {
        if (string.IsNullOrEmpty(Url)) return; // Can never happen is forced to be not null by CliFX
        var cancellation = console.RegisterCancellationHandler();
        var feed = await _feedClient.GetAsync(Url, cancellation);
        console.Output.WriteLine("✅ Downloaded feed with {0} items", feed?.Items?.Length);
        await WriteToFiles(feed, console, cancellation);
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

        foreach (var filename in Files)
        {
            await WriteToFile(md, filename, console, cancellationToken);
        }
    }

    private async Task WriteToFile(string newContent, string filename, IConsole console, CancellationToken cancellationToken)
    {
        var absolutePath = FileService.ConvertToAbsolute(filename);

        var result = await _contentService.ReplaceTextBetweenAsync(absolutePath, $"<!-- start {Tag} -->", $"<!-- end {Tag} -->", newContent, cancellationToken);

        if (result)
        {
            await console.Output.WriteLineAsync($"✔ Modifying file {absolutePath}");
        }
        else
        {
            await console.Output.WriteLineAsync($"❌ Modifying file {absolutePath}");
        }
    }
}