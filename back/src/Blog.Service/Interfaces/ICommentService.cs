using Blog.Service.DTOs.Comments;

namespace Blog.Service.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentResponse>> GetByPostIdAsync(int postId);
    Task<CommentResponse> CreateAsync(int postId, CreateCommentRequest request, int authorId);
    Task DeleteAsync(int commentId, int userId, string userRole);
}