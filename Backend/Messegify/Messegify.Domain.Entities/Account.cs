using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class Account : Entity
{ 
    [Key] 
    public Guid Id { get; set; }
    
    [MinLength(3), MaxLength(32)]
    public string Name { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;
    
    public DateTime DateCreated { get; set; }

    [ForeignKey(nameof(AccountChatroom.AccountId))]
    public virtual ICollection<AccountChatroom> AccountRooms { get; set; } = null!;
}