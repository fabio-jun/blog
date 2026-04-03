using Blog.Application.DTOs.Posts;

namespace Blog.Application.Interfaces;

public interface IPostService
{
    // Returns all posts (most recent first), with like info for the current user
    Task<IEnumerable<PostResponse>> GetAllAsync(int? currentUserId = null);
    // Returns a single post by ID, with like info for the current user
    Task<PostResponse> GetByIdAsync(int id, int? currentUserId = null);
    // Creates a new post (max 280 characters)
    Task<PostResponse> CreateAsync(CreatePostRequest request, int authorId);
    // Updates a post (author or admin only)
    Task<PostResponse> UpdateAsync(int postId, UpdatePostRequest request, int userId, string userRole);
    // Deletes a post (author or admin only)
    Task DeleteAsync(int postId, int userId, string userRole);
}
