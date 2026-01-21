using Microsoft.EntityFrameworkCore;
using Personal_Blog.Model.Domain;

namespace Personal_Blog.Contexts;

public class ArticleContext(DbContextOptions<ArticleContext> options) : DbContext(options)
{
    public DbSet<Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasSequence<int>("OrderArticleSeq")
            .StartsAt(1000000000) // 10
            .IncrementsBy(1);
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(article => article.Title)
                .IsRequired();

            entity.Property(article => article.Date)
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            entity.Property(article => article.Id)
                .HasDefaultValueSql("nextval('\"OrderArticleSeq\"')::text")
                .HasMaxLength(10);
        });
    }
}