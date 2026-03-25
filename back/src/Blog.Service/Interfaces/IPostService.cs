using Blog.Service.DTOs.Posts;

namespace Blog.Service.Interfaces;

public interface IPostService
{
    // Returns all posts (most recent first)
    Task<IEnumerable<PostResponse>> GetAllAsync();
    // Returns a single post by ID
    Task<PostResponse> GetByIdAsync(int id);
    // Creates a new post (max 280 characters)
    Task<PostResponse> CreateAsync(CreatePostRequest request, int authorId);
    // Updates a post (author or admin only)
    Task<PostResponse> UpdateAsync(int postId, UpdatePostRequest request, int userId, string userRole);
    // Deletes a post (author or admin only)
    Task DeleteAsync(int postId, int userId, string userRole);
}
