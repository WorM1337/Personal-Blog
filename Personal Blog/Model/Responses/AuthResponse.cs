namespace Personal_Blog.Model.Responses;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public string Role { get; set; } = string.Empty;
}