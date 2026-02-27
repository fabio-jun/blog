using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Service.DTOs.Posts;
using Blog.Service.Interfaces;

namespace Blog.Service.Services;

public class PostService : IPostService
{

    //External dependency field used for dependency injection
    private readonly IPostRepository _postRepository;

    //Constructor: DI
    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<PostResponse>> GetAllPublishedAsync()
    {
        var posts = await _postRepository.GetAllPublishedAsync();

        // Iterates each post and converts it to a PostResponse DTO
        // LINQ -.Select(): extension method from LINQ that transforms each element from a collection
        // Object Initializer - new PostResponse{ ... }: creates an object and define its properties without the need of an constructor with parameters
        return posts.Select(post => new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Slug = post.Slug,
            IsPublished = post.IsPublished,
            CreatedAt = post.CreatedAt,
            AuthorId = post.AuthorId,
            AuthorName = post.Author?.UserName ?? string.Empty
        });
    }

    public async Task<PostResponse> GetBySlugAsync(string slug)
    {
        var post = await _postRepository.GetBySlugAsync(slug);
        if (post == null)
        {
            throw new KeyNotFoundException("Post not found");
        }

        return new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Slug = post.Slug,
            IsPublished = post.IsPublished,
            CreatedAt = post.CreatedAt,
            AuthorId = post.AuthorId,
            AuthorName = post.Author?.UserName ?? string.Empty
        };
    }

    public async Task<PostResponse> CreateAsync(CreatePostRequest request, int authorId)
    {
        var post = new Post
        {
            Title = request.Title,
            Content = request.Content,
            Slug = GenerateSlug(request.Title),
            IsPublished = request.IsPublished,
            CreatedAt = DateTime.UtcNow,
            AuthorId = authorId
        };

        // Save at the DB
        await _postRepository.AddAsync(post);

        // Converts the entity back to DTO and returns 
        return new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Slug = post.Slug,
            IsPublished = post.IsPublished,
            CreatedAt = post.CreatedAt,
            AuthorId = post.AuthorId,
            AuthorName = string.Empty
        };
    }

    public async Task<PostResponse> UpdateAsync(int postId, UpdatePostRequest request, int userId, string userRole)
    {
        var post = await _postRepository.GetByIdAsync(postId);

        if(post == null)
        {
            throw new KeyNotFoundException("Post not found.");
        }
        
        if(post.AuthorId != userId && userRole != "Admin")
        {
            throw new KeyNotFoundException("Not authorized.");
        }

        post.Title = request.Title;
        post.Content = request.Content;
        post.Slug = GenerateSlug(request.Title);
        post.IsPublished = request.IsPublished;
        post.UpdatedAt = DateTime.UtcNow;

        await _postRepository.UpdateAsync(post);

        return new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Slug = post.Slug,
            IsPublished = post.IsPublished,
            CreatedAt = post.CreatedAt,
            AuthorId = post.AuthorId,
            AuthorName = post.Author?.UserName ?? string.Empty
        };  
    }

    public async Task DeleteAsync(int postId, int userId, string userRole)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if(post == null)
        {
            throw new KeyNotFoundException("Post not found.");
        }

        if(post.AuthorId != userId && userRole != "Admin")
        {
            throw new KeyNotFoundException("Not authorized.");
        }

        await _postRepository.DeleteAsync(post);
    }

    private string GenerateSlug(string title)
    {
      return title
          .ToLower()
          .Replace(" ", "-")
          .Replace("ã", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
          .Replace("ç", "c").Replace("â", "a").Replace("ê", "e").Replace("ô", "o")
          .Replace("à", "a");
   }
}