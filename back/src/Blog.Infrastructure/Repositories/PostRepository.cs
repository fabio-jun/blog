using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    // Returns all posts ordered by most recent first
    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await _context.Posts
            .Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    // Returns a single post by ID
    public async Task<Post?> GetByIdAsync(int id)
    {
        return await _context.Posts
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Inserts a new post
    public async Task AddAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    // Updates an existing post
    public async Task UpdateAsync(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    // Deletes a post
    public async Task DeleteAsync(Post post)
    {
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
    }
}
