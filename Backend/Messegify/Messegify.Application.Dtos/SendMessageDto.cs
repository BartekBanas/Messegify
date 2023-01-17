using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class SendMessageDto
{
    [Required]
    public string TextContent { get; set; }
}