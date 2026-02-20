using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Blog.Data;

//DbContext: EF Core's base class
public class AppDbContext : DbContext
{
    // Constructor receives the connection configurations
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Set tables as object collections in C#
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    //Apply the entities's classes configurations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
