using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class AccountChatRoom : Entity
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;

    [ForeignKey(nameof(Chatroom))]
    public Guid RoomId { get; set; }
    public Chatroom Chatroom { get; set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateJoined { get; set; }
}