using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllPublishedAsync();
    Task<Post?> GetBySlugAsync(string slug);
    Task<Post?> GetByIdAsync(int id);
    Task AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(Post post);


}