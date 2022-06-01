using System.Text.Json.Serialization;

namespace Feeder.Models
{
  public record Item {
    public string? Id { get; set; }
    public string? Url { get; set; }
    public string? Title { get; set; }
    [JsonPropertyName("content_html")]
    public string? ContentHtml { get; set; }
    public string? Summary { get; set; }
    [JsonPropertyName("date_published")]
    public DateTimeOffset? DatePublished { get; set; }
    public string[]? Tags { get; set; }

    internal string GetFormatted(string template) {
      return template
        .Replace("{title}", Title, StringComparison.InvariantCultureIgnoreCase)
        .Replace("{url}", Url, StringComparison.InvariantCultureIgnoreCase)
        .Replace("{dateIso}", DatePublished?.ToString("yyyy-MM-dd"), StringComparison.InvariantCultureIgnoreCase)
        .Replace("{date}", DatePublished?.ToString("dd-MM-yy"), StringComparison.InvariantCultureIgnoreCase);
    }
  }
}