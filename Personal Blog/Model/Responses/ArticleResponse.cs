namespace Personal_Blog.Model.Responses;

public class ArticleResponse
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public DateTime? Date { get; set; }
    public string? Text { get; set; } = null;
}