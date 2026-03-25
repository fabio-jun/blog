using Blog.Service.DTOs.Posts;
using Blog.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Blog.API.Controllers;

// Attribute that enables API behaviors
[ApiController]
// Defines controller's base route
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    // External dependency used for dependency injection
    private readonly IPostService _postService;

    // Constructor: DI
    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    // GET api/post — public, returns all posts (most recent first)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _postService.GetAllAsync();
        return Ok(posts);
    }

    // GET api/post/{id} — public, returns a single post
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var post = await _postService.GetByIdAsync(id);
        return Ok(post);
    }

    // POST api/post — authenticated, creates a new post
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreatePostRequest request)
    {
        // Extracts the user ID from the JWT token claims
        var authorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var post = await _postService.CreateAsync(request, authorId);
        return Ok(post);
    }

    // PUT api/post/{id} — authenticated, author or admin can update
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdatePostRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var userRole = User.FindFirst(ClaimTypes.Role)!.Value;

        var post = await _postService.UpdateAsync(id, request, userId, userRole);
        return Ok(post);
    }

    // DELETE api/post/{id} — authenticated, author or admin can delete
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var userRole = User.FindFirst(ClaimTypes.Role)!.Value;

        await _postService.DeleteAsync(id, userId, userRole);
        // Returns HTTP 204 (No Content) — standard for successful DELETE
        return NoContent();
    }
}
