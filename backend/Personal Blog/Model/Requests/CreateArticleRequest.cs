namespace Personal_Blog.Model.Requests;

public class CreateArticleRequest
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string? Text { get; set; } = null;
}