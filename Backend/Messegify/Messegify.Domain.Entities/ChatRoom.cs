using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class ChatRoom : Entity
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    [Column(TypeName = "nvarchar(24)")]
    public ChatRoomType ChatRoomType { get; set; }

    public virtual ICollection<AccountChatRoom> Members { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
}

public enum ChatRoomType
{
    Direct,
    Normal
}