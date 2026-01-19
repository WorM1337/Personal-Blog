using Personal_Blog.Model.Exceptions;
using Personal_Blog.Model.Requests;
using Personal_Blog.Model.Responses;
using Personal_Blog.Services;

namespace Personal_Blog.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController(IConfiguration configuration, AuthService authService) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly AuthService _authService = authService;
    
    
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = _authService.Login(request);
            
            return Ok(result);
        }
        catch (BadLoginException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (IncorrectLoginException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
    
    [HttpGet("check")]
    [Authorize]
    public IActionResult CheckAuth()
    {
        var username = User.Identity?.Name;
        return Ok(new CheckAuthResponse
        {
            Username = username,
            Claims = User.Claims.Select(c => (c.Type, c.Value))
        });
    }
    
    
}