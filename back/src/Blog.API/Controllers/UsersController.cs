using System.Security.Claims;
using Blog.Application.DTOs.Users;
using Blog.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Blog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    // GET api/users/{id} - public, returns a users's profile
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    // PUT api/users/me - private, updates own profile
    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMe(UpdateUserRequest request)
    {
        // Extracts the user ID from the JWT token claims
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await _userService.UpdateAsync(userId, request);
        return Ok(user);
    }

    
}