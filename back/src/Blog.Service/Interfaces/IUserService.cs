using Blog.Service.DTOs.Users;

namespace Blog.Service.Interfaces;

public interface IUserService
{
    // Return a users's public profile
    Task<UserResponse> GetByIdAsync(int id);

    //Updates the authenticated user's profile
    Task<UserResponse> UpdateAsync(int userId, UpdateUserRequest request);
}