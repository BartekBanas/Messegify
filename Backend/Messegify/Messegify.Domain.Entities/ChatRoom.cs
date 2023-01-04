using System.ComponentModel.DataAnnotations;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class ChatRoom : Entity
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public virtual ICollection<AccountChatRoom> Members { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
}