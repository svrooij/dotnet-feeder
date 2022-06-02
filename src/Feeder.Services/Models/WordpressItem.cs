namespace Feeder.Services.Models;
public record WordpressItem {
  public int Id { get; set; }
  public WordpressTitle Title { get; set; }
  public string Link { get; set; }
}

public record WordpressTitle {
  public string Rendered {get;set;}
}
