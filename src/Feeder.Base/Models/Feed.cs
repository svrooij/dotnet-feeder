namespace Feeder.Base.Models;

public record Feed
{
    public string? Version { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Item[]? Items { get; set; }
}