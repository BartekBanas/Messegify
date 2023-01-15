using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class RegisterAccountDto
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string Email { get; set; }
}