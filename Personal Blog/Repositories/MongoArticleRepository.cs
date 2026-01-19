using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Personal_Blog.Model.Domain;
using Personal_Blog.Model.Settings;

namespace Personal_Blog.Repositories;

public class MongoArticleRepository: IArticleRepository
{
    private readonly IMongoCollection<Article> _articlesCollection;
    private readonly ILogger<MongoArticleRepository> _logger;
    public MongoArticleRepository(IOptions<ArticlesDatabaseSetttings> articlesDatabaseSettings, ILogger<MongoArticleRepository> logger)
    {
        _logger = logger;
        
        var connectionString = articlesDatabaseSettings.Value.ConnectionString;
        var databaseName = articlesDatabaseSettings.Value.DatabaseName;
        var collectionName = articlesDatabaseSettings.Value.CollectionName;
        if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName) ||
            string.IsNullOrEmpty(collectionName))
        {
            throw new ArgumentException("Connection and DatabaseName and CollectionName are required in configuration settings");
        }
        
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        try
        {
            database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            _logger.LogInformation("Ping database '{DatabaseName}': Success", databaseName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"MongoDB connection failed: {ex.Message}", ex);
        }
        
        _articlesCollection = database.GetCollection<Article>(collectionName);
    }
    
    public async Task<Article> InsertAsync(string title, DateTime date, string? text)
    {
        var article = new Article()
        {
            Title = title,
            Date = date,
            Text = text
        };
        await _articlesCollection.InsertOneAsync(article);
        return  article;
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        return await _articlesCollection.AsQueryable().ToListAsync();
    }

    public async Task<Article?> GetByIdAsync(string id)
    {
        return await _articlesCollection.Find(art => art.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Article?> UpdateAsync(string id, string? title, DateTime? date, string? text)
    {
        var filter = Builders<Article>.Filter.Eq(art => art.Id, id);
        
        var updateList = new List<UpdateDefinition<Article>>();

        if (title != null)
        {
            updateList.Add(Builders<Article>.Update.Set("Title", title));
        }
        if (date != null)
        {
            updateList.Add(Builders<Article>.Update.Set("Date", date));
        }
        if (text != null)
        {
            updateList.Add(Builders<Article>.Update.Set("Text", text));
        }
        
        var combinedUpdate = Builders<Article>.Update.Combine(updateList);
        
        var upsertOptions = new FindOneAndUpdateOptions<Article>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = true
        };

        var result = await _articlesCollection.FindOneAndUpdateAsync(filter, combinedUpdate, upsertOptions);
        return result;
    }

    public async Task<Article?> DeleteAsync(string id)
    {
        var filter = Builders<Article>.Filter.Eq(art => art.Id, id);
        var result = await _articlesCollection.FindOneAndDeleteAsync(filter);
        return result;
    }
}