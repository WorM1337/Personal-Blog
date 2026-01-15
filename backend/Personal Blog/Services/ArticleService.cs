using System.Reflection.Metadata;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Personal_Blog.Model.Domain;
using Personal_Blog.Model.Requests;
using Personal_Blog.Model.Settings;

namespace Personal_Blog.Services;

public class ArticleService
{
    public readonly IMongoCollection<Article> _articlesCollection;
    public ArticleService(IOptions<ArticlesDatabaseSetttings> articlesDatabaseSettings)
    {
        var client = new MongoClient(articlesDatabaseSettings.Value.ConnectionString);
        var database = client.GetDatabase(articlesDatabaseSettings.Value.DatabaseName);
        _articlesCollection = database.GetCollection<Article>(articlesDatabaseSettings.Value.CollectionName);
    }

    public async Task InsertArticle(CreateArticleRequest request)
    {
        
        await _articlesCollection.InsertOneAsync(new Article()
        {
            Title = request.Title,
            Date = request.Date,
            Text = request.Text,
        });
    }

    public async Task<IEnumerable<Article>> GetAll()
    {
        return await _articlesCollection.AsQueryable().ToListAsync();
    }

    public async Task<Article?> GetById(string id)
    {
        return await _articlesCollection.Find(art => art.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateOne(string id, UpdateArticleRequest request)
    {
        var filter = Builders<Article>.Filter.Eq(art => art.Id, id);
        
        var updateList = new List<UpdateDefinition<Article>>();

        if (request.Title != null)
        {
            updateList.Add(Builders<Article>.Update.Set("Title", request.Title));
        }
        if (request.Date != null)
        {
            updateList.Add(Builders<Article>.Update.Set("Date", request.Date));
        }
        if (request.Text != null)
        {
            updateList.Add(Builders<Article>.Update.Set("Text", request.Text));
        }
        
        var combinedUpdate = Builders<Article>.Update.Combine(updateList);

        var result = await _articlesCollection.UpdateOneAsync(filter, combinedUpdate);
        return result.IsAcknowledged && result.MatchedCount == 1;
    }

    public async Task<bool> DeleteById(string id)
    {
        var filter = Builders<Article>.Filter.Eq(art => art.Id, id);
        var result = await _articlesCollection.DeleteOneAsync(filter);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}