using Messegify.Domain.Entities;

namespace Messegify.Application.Dtos;

public class ChatRoomDto
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public ChatRoomType ChatRoomType { get; set; }
    
    // public virtual ICollection<AccountChatroom> Members { get; set; }
}