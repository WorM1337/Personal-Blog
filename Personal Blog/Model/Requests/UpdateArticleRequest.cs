namespace Personal_Blog.Model.Requests;

public class UpdateArticleRequest
{
    public string? Title { get; set; } =  null;
    public DateTime? Date { get; set; } = null;
    public string? Text { get; set; } = null;
}