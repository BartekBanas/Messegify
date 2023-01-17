using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class MessageDto
{
    public Guid Id { get; set; }
    
    public DateTime SentDate { get; set; }
    
    public Guid AccountId { get; set; }
    
    [Required]
    public string TextContent { get; set; } = null!;
}