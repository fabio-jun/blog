//Encapsulate the access to the DB

using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repositories;

public class UserRepository : IUserRepository
{
    //Field of type DbContext
    private readonly AppDbContext _context;

    //Construtor
    //DI: _context receives an instance of AppDbConext
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    //Find async only works with PK (more efficient than FirstOrDefault)
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    //Searches for the first register that meets the lambda
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }
    public async Task<User?> GetByEmailAsync(string Email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
    }

    //Converts the query into a list
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}