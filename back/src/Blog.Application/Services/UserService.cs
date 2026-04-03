using Blog.Domain.Interfaces;
using Blog.Application.DTOs.Users;
using Blog.Application.Interfaces;

namespace Blog.Application.Services;

public class UserService : IUserService
{
    // External dependency field used for DI
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Receives an usr ID and returns a userResponse DTO
    public async Task<UserResponse> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if(user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        //Converts the entity to a DTO
        return new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }

    // Updates user profile and return the updated UserResponse DTO
    public async Task<UserResponse> UpdateAsync(int userId, UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if(user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        // Updates the entity fields with the request data
        user.UserName =request.userName;
        user.Email = request.Email;

        // Saves changes to the #pragma warning disable format
        await _userRepository.UpdateAsync(user);

        // Converts the entity back to DTO and returns
        return new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }
}