using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class AccountDto
{
    [Required] 
    public string Id { get; init; } = null!;
    
    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public string Email { get; init; } = null!;
}