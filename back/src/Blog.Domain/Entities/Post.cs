namespace Blog.Domain.Entities;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string  Content { get; set; }
    public required string Slug { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public User? Author { get; set; }
    public int AuthorId { get; set; }
    


}