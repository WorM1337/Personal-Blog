using System.Reflection.Metadata;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Personal_Blog.Model.Domain;
using Personal_Blog.Model.Exceptions;
using Personal_Blog.Model.Requests;
using Personal_Blog.Model.Responses;
using Personal_Blog.Model.Settings;
using Personal_Blog.Repositories;

namespace Personal_Blog.Services;

public class ArticleService(IArticleRepository articleRepository)
{
    private readonly IArticleRepository _articleRepository = articleRepository;


    public int GetRequiredIdLength()
    {
        return _articleRepository.GetRequiredIdLength();
    }
    public async Task<ArticleResponse> InsertArticle(CreateArticleRequest request)
    {
        var result = await _articleRepository.InsertAsync(request.Title, request.Date, request.Text);

        return new ArticleResponse()
        {
            Date = request.Date,
            Text = request.Text,
            Title = request.Title,
            Id = result.Id
        };
    }

    public async Task<IEnumerable<ArticleResponse>> GetAll()
    {
        var result = await _articleRepository.GetAllAsync();
        return result.Select(article => new ArticleResponse()
        {
            Date = article.Date,
            Text = article.Text,
            Title = article.Title,
            Id = article.Id
        });
    }

    public async Task<ArticleResponse?> GetById(string id)
    {
        var result = await _articleRepository.GetByIdAsync(id);
        
        if(result == null)
            throw new ArticleNotfoundException("Article not found by id");
        
        return new ArticleResponse()
        {
            Id = result.Id,
            Date = result.Date,
            Text = result.Text,
            Title = result.Title,
        };
    }

    public async Task<ArticleResponse> UpdateOne(string id, UpdateArticleRequest request)
    {
        var result = await _articleRepository.UpdateAsync(id,  request.Title, request.Date, request.Text);
        
        if(result == null)
            throw new ArticleNotfoundException("Article not found by id");
        
        return new ArticleResponse()
        {
            Id = result.Id,
            Date = result.Date,
            Text = result.Text,
            Title = result.Title,
        };
    }

    public async Task<ArticleResponse> DeleteById(string id)
    {
        var result = await _articleRepository.DeleteAsync(id);
        
        if(result == null)
            throw new ArticleNotfoundException("Article not found by id");
        
        return new ArticleResponse()
        {
            Id = result.Id,
            Date = result.Date,
            Text = result.Text,
            Title = result.Title,
        };
    }
}