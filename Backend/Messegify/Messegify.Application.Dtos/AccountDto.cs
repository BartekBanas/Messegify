using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class AccountDto
{
    [Required] 
    public string Id { get; set; } = null!;
    
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}