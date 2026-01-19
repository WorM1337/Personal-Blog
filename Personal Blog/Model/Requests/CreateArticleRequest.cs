using System.ComponentModel.DataAnnotations;

namespace Personal_Blog.Model.Requests;

public class CreateArticleRequest
{
    [Required(ErrorMessage = "Title is required")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }
    public string? Text { get; set; } = null;
}