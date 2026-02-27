using Blog.Service.DTOs.Posts;
using Blog.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Blog.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _postService.GetAllPublishedAsync();
        return Ok(posts); // 200 + JSON
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var post = await _postService.GetBySlugAsync(slug);
        return Ok(post);
    }

    [HttpPost]
    [Authorize] // Protects the route with JWT
    public async Task<IActionResult> Create(CreatePostRequest request)
    {
        var authorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var post = await _postService.CreateAsync(request, authorId);
        return Ok(post);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdatePostRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var userRole = User.FindFirst(ClaimTypes.Role)!.Value;

        var post = await _postService.UpdateAsync(id, request, userId, userRole);
        return Ok(post);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var userRole = User.FindFirst(ClaimTypes.Role)!.Value;

        await _postService.DeleteAsync(id, userId, userRole);
        return NoContent(); // 204
    }

}

