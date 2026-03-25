namespace Blog.Domain.Entities;

public class Post
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public User? Author { get; set; }
    public int AuthorId { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public string? ImageUrl { get; set; }
}
