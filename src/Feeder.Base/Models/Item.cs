using System.Text.Json.Serialization;

namespace Feeder.Base.Models;

public record Item
{
    public string? Id { get; set; }
    public string? Url { get; set; }
    public string? Title { get; set; }
    [JsonPropertyName("content_html")]
    public string? ContentHtml { get; set; }
    public string? Summary { get; set; }
    [JsonPropertyName("date_published")]
    public DateTimeOffset? DatePublished { get; set; }
    public string[]? Tags { get; set; }
}

public static class ItemExtensions
{
    public static string FormatItem(this Item item, string template)
    {
        return template
            .Replace("{title}", item.Title, StringComparison.InvariantCultureIgnoreCase)
            .Replace("{url}", item.Url, StringComparison.InvariantCultureIgnoreCase)
            .Replace("{dateIso}", item.DatePublished?.ToString("yyyy-MM-dd"), StringComparison.InvariantCultureIgnoreCase)
            .Replace("{date}", item.DatePublished?.ToString("dd-MM-yy"), StringComparison.InvariantCultureIgnoreCase)
            .Replace("{tags}", item.Tags != null ? string.Join(", ", item.Tags) : "");
    }
}