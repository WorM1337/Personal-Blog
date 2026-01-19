using System.ComponentModel.DataAnnotations;

namespace Personal_Blog.Model.Requests;

public class LoginRequest
{
    [Required(ErrorMessage = "Login is required")]
    public string Login { get; set; } = "";
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}