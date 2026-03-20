namespace Blog.Service.DTOs.Users;

// DTO for returning user data to the client
public class UserResponse
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

