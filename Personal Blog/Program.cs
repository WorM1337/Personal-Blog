using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Personal_Blog.Contexts;
using Personal_Blog.Model.Settings;
using Personal_Blog.Repositories;
using Personal_Blog.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =  JsonNamingPolicy.CamelCase;
    });

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();

builder.Services.Configure<ArticlesDatabaseSettings>(
    builder.Configuration.GetSection("ArticlesMongoDBSettings"));

if (builder.Configuration["CurrentDatabase"] == "PostgreSQL")
{
    builder.Services.AddDbContext<ArticleContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
    builder.Services.AddScoped<IArticleRepository, PostgresArticleRepository>();
}
else
{
    builder.Services.AddSingleton<IArticleRepository, MongoArticleRepository>();
}



builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddLogging();


var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();