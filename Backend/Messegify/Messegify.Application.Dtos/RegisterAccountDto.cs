using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class RegisterAccountDto(string name, string password, string email)
{
    [Required]
    public string Name { get; init; } = name;

    [Required]
    public string Password { get; init; } = password;

    [Required]
    public string Email { get; init; } = email;
}