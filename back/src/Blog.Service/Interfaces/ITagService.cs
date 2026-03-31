using Blog.Service.DTOs.Tags;

namespace Blog.Service.Interfaces;

public interface ITagService
{
    // Return all tags ordered by popularity
    Task<IEnumerable<TagResponse>> GetAllAsync();
}