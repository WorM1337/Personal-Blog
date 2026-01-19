using Personal_Blog.Model.Domain;
using Personal_Blog.Model.Requests;

namespace Personal_Blog.Repositories;

public interface IArticleRepository
{
    Task<Article> InsertAsync(string title, DateTime date, string? text);
    Task<IEnumerable<Article>> GetAllAsync();
    Task<Article?> GetByIdAsync(string id);
    Task<Article?> UpdateAsync(string id, string? title, DateTime? date, string? text);
    Task<Article?> DeleteAsync(string id);
}