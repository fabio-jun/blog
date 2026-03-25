using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Service.DTOs.Posts;
using Blog.Service.Interfaces;

namespace Blog.Service.Services;

public class PostService : IPostService
{
    // External dependency field used for dependency injection
    private readonly IPostRepository _postRepository;

    // Constructor: DI
    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    // Returns all posts (most recent first)
    public async Task<IEnumerable<PostResponse>> GetAllAsync()
    {
        var posts = await _postRepository.GetAllAsync();

        return posts.Select(post => new PostResponse
        {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            AuthorId = post.AuthorId,
            AuthorName = post.Author?.UserName ?? string.Empty,
            ImageUrl = post.ImageUrl
        });
    }

    // Returns a single post by ID
    public async Task<PostResponse> GetByIdAsync(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            throw new KeyNotFoundException("Post not found.");

        return new PostResponse
        {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            AuthorId = post.AuthorId,
            AuthorName = post.Author?.UserName ?? string.Empty,
            ImageUrl = post.ImageUrl
        };
    }

    // Creates a new post (max 280 characters)
    public async Task<PostResponse> CreateAsync(CreatePostRequest request, int authorId)
    {
        if (request.Content.Length > 280)
            throw new ArgumentException("Post must be 280 characters or less.");

        var post = new Post
        {
            Content = request.Content,
            CreatedAt = DateTime.UtcNow,
            AuthorId = authorId,
            ImageUrl = request.ImageUrl
        };

        await _postRepository.AddAsync(post);

        return new PostResponse
        {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            AuthorId = post.AuthorId,
            AuthorName = string.Empty,
            ImageUrl = post.ImageUrl
        };
    }

    // Updates a post (author or admin only)
    public async Task<PostResponse> UpdateAsync(int postId, UpdatePostRequest request, int userId, string userRole)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
            throw new KeyNotFoundException("Post not found.");

        if (post.AuthorId != userId && userRole != "Admin")
            throw new UnauthorizedAccessException("Not authorized.");

        if (request.Content.Length > 280)
            throw new ArgumentException("Post must be 280 characters or less.");

        post.Content = request.Content;
        post.ImageUrl = request.ImageUrl;
        post.UpdatedAt = DateTime.UtcNow;

        await _postRepository.UpdateAsync(post);

        return new PostResponse
        {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            AuthorId = post.AuthorId,
            AuthorName = post.Author?.UserName ?? string.Empty,
            ImageUrl = post.ImageUrl
        };
    }

    // Deletes a post (author or admin only)
    public async Task DeleteAsync(int postId, int userId, string userRole)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
            throw new KeyNotFoundException("Post not found.");

        if (post.AuthorId != userId && userRole != "Admin")
            throw new UnauthorizedAccessException("Not authorized.");

        await _postRepository.DeleteAsync(post);
    }
}
