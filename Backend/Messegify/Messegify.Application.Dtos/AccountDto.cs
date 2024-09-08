using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class AccountDto
{
    [Required] 
    public required string Id { get; init; }
    
    [Required]
    public required string Name { get; init; }

    [Required]
    public required string Email { get; init; }
}