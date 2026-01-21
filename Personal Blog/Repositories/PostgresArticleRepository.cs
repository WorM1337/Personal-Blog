using Microsoft.EntityFrameworkCore;
using Personal_Blog.Contexts;
using Personal_Blog.Model.Domain;

namespace Personal_Blog.Repositories;

public class PostgresArticleRepository(ArticleContext articleContext) : IArticleRepository
{
    private readonly ArticleContext _articleContext = articleContext;
    public async Task<Article> InsertAsync(string title, DateTime date, string? text)
    {
        var newArticle = new Article()
        {
            Title = title,
            Date = date,
            Text = text
        };
        await _articleContext.Articles.AddAsync(newArticle);
        await _articleContext.SaveChangesAsync();
        return newArticle;
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        return _articleContext.Articles.AsEnumerable();
    }

    public async Task<Article?> GetByIdAsync(string id)
    {
        return  _articleContext.Articles.FirstOrDefault(a => a.Id == id);
    }

    public async Task<Article?> UpdateAsync(string id, string? title, DateTime? date, string? text)
    {
        var  article = await _articleContext.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article != null)
        {
            if (title != null)
            {
                article.Title = title;
            }
            if (date != null)
            {
                article.Date = date.Value;
            }
            if (text != null)
            {
                article.Text = text;
            }
            await _articleContext.SaveChangesAsync();
        }
        return article;
    }

    public async Task<Article?> DeleteAsync(string id)
    {
        var article = await _articleContext.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article != null)
        {
            _articleContext.Articles.Remove(article);
            await _articleContext.SaveChangesAsync();
        }
        return article;
    }

    public bool IsRequiredIdLength(int length)
    {
        return length == 10;
    }
}