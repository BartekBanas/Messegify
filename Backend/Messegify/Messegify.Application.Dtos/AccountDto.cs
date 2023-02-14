using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class AccountDto
{
    [Required] 
    public string Id { get; set; } = null!;
    
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;
}