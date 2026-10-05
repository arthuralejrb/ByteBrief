using System;
using ByteBrief.Worker.models;
using Microsoft.EntityFrameworkCore;

namespace ByteBrief.Worker.data;

public class ByteBriefContext : DbContext
{
    public DbSet<NewsArticle> NewsArticles {get; set;}

    public ByteBriefContext(DbContextOptions<ByteBriefContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<NewsArticle>()
            .HasIndex(a => a.Url)
            .IsUnique();

    }

}
