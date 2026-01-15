using System.Text.Json;
using Personal_Blog.Model.Settings;
using Personal_Blog.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =  JsonNamingPolicy.CamelCase;
    });

builder.Services.Configure<ArticlesDatabaseSetttings>(
    builder.Configuration.GetSection("ArticlesDatabaseSettings"));
builder.Services.AddScoped<ArticleService>();
builder.Services.AddLogging();

var app = builder.Build();

app.MapControllers();
app.Run();