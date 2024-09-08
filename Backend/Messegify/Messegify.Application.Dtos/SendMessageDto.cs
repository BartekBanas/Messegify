using System.ComponentModel.DataAnnotations;

namespace Messegify.Application.Dtos;

public class SendMessageDto(string textContent)
{
    [Required]
    public string TextContent { get; } = textContent;
}