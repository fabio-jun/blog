using Blog.Service.DTOs.Posts;

namespace Blog.Service.Interfaces;

public interface IPostService
{
    Task<IEnumerable<PostResponse>> GetAllPublishedAsync();
    Task<PostResponse> GetBySlugAsync(string slug);
    Task<PostResponse> CreateAsync(CreatePostRequest request, int authorId);
    Task<PostResponse> UpdateAsync(int postId, UpdatePostRequest request, int userId, string userRole);
    Task DeleteAsync(int postId, int userId, string userRole);
    

}