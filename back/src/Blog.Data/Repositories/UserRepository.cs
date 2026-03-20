// Encapsulate the access to the DB
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repositories;

public class UserRepository : IUserRepository
{
    // Field of type DbContext
    private readonly AppDbContext _context;

    // Construtor
    // DI: _context receives an instance of AppDbConext
    public UserRepository(AppDbContext context)
    {
        // Instance of the AppDbContext
        _context = context;
    }

    // Find async only works with PK (more efficient than FirstOrDefault)
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    // Searches for the first register that meets the lambda
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }
    public async Task<User?> GetByEmailAsync(string Email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
    }

    // Converts the query into a list and return all users
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    // Adds a new user to the database and saves the changes
    public async Task AddAsync(User user)
    {
        // AddAsync() becomes an INSERT
        await _context.Users.AddAsync(user);
        // SaveChangesAsync() executes the SQL commands in the DB
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        // Update becomes an UPDATE
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        // Remove becomes a DELETE
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}