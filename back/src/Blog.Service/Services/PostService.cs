using System.Text.RegularExpressions;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Service.DTOs.Posts;
using Blog.Service.Interfaces;

namespace Blog.Service.Services;

public class PostService : IPostService
{
    // External dependencies used for dependency injection
    private readonly IPostRepository _postRepository;
    private readonly ITagRepository _tagRepository;

    // Constructor: DI
    public PostService(IPostRepository postRepository, ITagRepository tagRepository)
    {
        _postRepository = postRepository;
        _tagRepository = tagRepository;
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

        // Extract hashtags from content and associate them with the post
        await ExtractAndSaveTags(post, request.Content);

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

        // Clear old tag associations and extract new ones
        post.PostTags?.Clear();
        await _postRepository.UpdateAsync(post);
        await ExtractAndSaveTags(post, request.Content);

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

    // Extracts hashtags from text and saves them as Tag + PostTag in the DB
    private async Task ExtractAndSaveTags(Post post, string content)
    {
        var hashtags = Regex.Matches(content, @"#(\w+)")
            .Select(m => m.Groups[1].Value.ToLower())
            .Distinct()
            .ToList();

        foreach (var tagName in hashtags)
        {
            // Check if the tag already exists in the DB
            var tag = await _tagRepository.GetByNameAsync(tagName);

            // If not, create a new one
            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                await _tagRepository.AddAsync(tag);
            }

            // Create the association between the post and the tag
            post.PostTags ??= new List<PostTag>();
            post.PostTags.Add(new PostTag { PostId = post.Id, TagId = tag.Id });
        }

        // Save the associations to the database
        await _postRepository.UpdateAsync(post);
    }
}
