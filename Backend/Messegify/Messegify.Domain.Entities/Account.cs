using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class Account : IEntity
{ 
    [Key] 
    public Guid Id { get; set; }
    
    [MinLength(3), MaxLength(32)]
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateCreated { get; set; }

    [ForeignKey(nameof(AccountRoom.AccountId))]
    public virtual ICollection<AccountRoom> AccountRooms { get; set; }
}