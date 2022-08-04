namespace F1Desktop.Models.News;

public class NewsItem
{
    public DateTimeOffset Published { get; init; }
    public string Provider { get; init; }
    public string Title { get; init; }
    public string Content { get; init; }
    public string Url { get; init; }
    public string ImageUrl { get; init; }
}