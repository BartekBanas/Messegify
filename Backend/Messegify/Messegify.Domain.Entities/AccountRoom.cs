using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Messegify.Domain.Entities;

public class AccountRoom : Entity
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    
    [ForeignKey(nameof(ChatRoom))]
    public Guid RoomId { get; set; }
    public ChatRoom ChatRoom { get; set; }

    public DateTime DateJoined { get; set; }
}