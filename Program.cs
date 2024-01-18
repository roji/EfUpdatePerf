using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

await using var ctx = new BlogContext();
await ctx.Database.EnsureDeletedAsync();
await ctx.Database.EnsureCreatedAsync();

ctx.Blogs.Add(new Blog
{
    Name = "MyBlog"
});
await ctx.SaveChangesAsync();

public class BlogContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer("Server=localhost;Database=test;User=SA;Password=Abcd5678;Connect Timeout=60;ConnectRetryCount=0;Encrypt=false")
            .LogTo(Console.WriteLine, new[]
            {
                RelationalEventId.TransactionCommitted,
                RelationalEventId.TransactionStarted,
                RelationalEventId.CommandExecuted
            })
            .EnableSensitiveDataLogging();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}

public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Rating { get; set; }
    
    public List<Post> Posts { get; set; }
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public int BlogId { get; set; }
}
