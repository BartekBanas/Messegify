using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class AccountDto(string id, string name, string email)
{
    [Required] 
    public string Id { get; init; } = id;

    [Required]
    public string Name { get; init; } = name;

    [Required]
    public string Email { get; init; } = email;
}