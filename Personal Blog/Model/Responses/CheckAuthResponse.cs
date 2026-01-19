using System.Security.Claims;

namespace Personal_Blog.Model.Responses;

public class CheckAuthResponse
{
    public string? Username { get; set; }
    public IEnumerable<(string, string)>? Claims { get; set; }
}