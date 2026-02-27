namespace Blog.Service.DTOs.Posts;

public class UpdatePostRequest
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public bool IsPublished { get; set; }
    
}