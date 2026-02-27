namespace Blog.Service.DTOs.Posts;

public class PostResponse
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Slug { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required int AuthorId { get; set; }
    public required string AuthorName { get; set; }
 


}