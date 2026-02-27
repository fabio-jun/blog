using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetAllPublishedAsync()
    {
        // SELECT * FROM "Posts"
        return await _context.Posts
        .Where(p => p.IsPublished)
        .Include(p => p.Author)
        .ToListAsync()
        ;
    }

    public async Task<Post?> GetBySlugAsync(string slug)
    {
        // SELECT * FROM "Posts" WHERE "Slug" = ...
        return await _context.Posts.FirstOrDefaultAsync( p => p.Slug == slug);
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        // SELECT * FROM "Posts" WHERE "Id" = ...
        return await _context.Posts.FindAsync(id);
    }

    public async Task AddAsync(Post post)
    {
        // INSERT INTO "Posts"...
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Post post)
    {
        // SELECT * FROM "Posts" WHERE Id = @id
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Post post)
    {
        // DELETE FROM "Posts" SET ...
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
    }




    
}