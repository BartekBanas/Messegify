using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class Chatroom : Entity
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;

    [Column(TypeName = "nvarchar(24)")]
    public ChatRoomType ChatRoomType { get; set; }

    public virtual ICollection<AccountChatroom> Members { get; set; } = null!;
    public virtual ICollection<Message> Messages { get; set; } = null!;
}

public enum ChatRoomType
{
    Direct,
    Regular
}