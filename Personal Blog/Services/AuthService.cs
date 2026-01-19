using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Personal_Blog.Model.Exceptions;
using Personal_Blog.Model.Requests;
using Personal_Blog.Model.Responses;

namespace Personal_Blog.Services;

public class AuthService(IConfiguration configuration, ILogger<AuthService> logger)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<AuthService> _logger = logger;
    
    public AuthResponse Login(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
        {
            _logger.LogInformation("Invalid login or password");
            throw new BadLoginException("Invalid login or password");
        }
        if (CheckLoginPassword(request.Login, request.Password))
        {
            var token = GenerateJwtToken(request.Login, isAdmin: true);
            var expireHours = int.Parse(_configuration["Jwt:ExpireHours"] ?? "2");
            
            _logger.LogInformation($"OK admin auth. Token: {token}");
            return new AuthResponse
            {
                Token = token,
                Expires = DateTime.UtcNow.AddHours(expireHours),
                Role = "Admin"
            };
        }
        _logger.LogInformation($"Incorrect login ({request.Login}) or password {request.Password}");
        throw new IncorrectLoginException("Incorrect login or password");
    }
    
    private bool CheckLoginPassword(string login, string password)
    {
        var loginAdmin = _configuration["AdminCredentials:Login"];
        var passwordAdmin = _configuration["AdminCredentials:Password"];
        return loginAdmin == login && password == passwordAdmin;
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