namespace Messegify.Application.Dtos;

public class LoginDto
{
    public string? UsernameOrEmail { get; set; }
    public string Password { get; set; }
}