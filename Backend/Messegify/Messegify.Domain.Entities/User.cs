using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class User : Entity
{
    [Key]
    public Guid Id { get; set; }
    
    public string Username { get; set; }

    public DateTime DateCreated { get; set; }
    
    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; }
}