using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class RegisterAccountDto
{
    [Required]
    public required string Name { get; init; }
    
    [Required]
    public required string Password { get; init; }
    
    [Required]
    public required string Email { get; init; }
}