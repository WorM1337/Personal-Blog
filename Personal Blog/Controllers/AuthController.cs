using Personal_Blog.Model.Requests;
using Personal_Blog.Model.Responses;

namespace Personal_Blog.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/auth")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { message = "Логин и пароль обязательны" });
        }
        
        var adminLogin = _configuration["AdminCredentials:Login"];
        var adminPassword = _configuration["AdminCredentials:Password"];
        
        if (request.Login == adminLogin && request.Password == adminPassword)
        {
            var token = GenerateJwtToken(request.Login, isAdmin: true);
            var expireHours = int.Parse(_configuration["Jwt:ExpireHours"] ?? "2");

            return Ok(new AuthResponse
            {
                Token = token,
                Expires = DateTime.UtcNow.AddHours(expireHours),
                Role = "Admin"
            });
        }
        return Unauthorized(new { message = "Неверный логин или пароль" });
    }
    
    [HttpGet("check")]
    [Authorize]
    public IActionResult CheckAuth()
    {
        var isAdmin = User.HasClaim("IsAdmin", "true");
        var username = User.Identity?.Name;

        return Ok(new
        {
            IsAuthenticated = true,
            Username = username,
            IsAdmin = isAdmin,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
    
    private string GenerateJwtToken(string username, bool isAdmin)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        };

        if (isAdmin)
        {
            claims.Add(new Claim("IsAdmin", "true"));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        var expireHours = int.Parse(_configuration["Jwt:ExpireHours"] ?? "2");
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expireHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}