using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Personal_Blog.Model.Domain;

public class Article
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonRequired]
    public string Title { get; set; }
    [BsonRepresentation(BsonType.DateTime)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
    [BsonRequired]
    public DateTime Date { get; set; } = DateTime.UtcNow;
    [BsonRepresentation(BsonType.String)]
    public string? Text { get; set; } = null;
}