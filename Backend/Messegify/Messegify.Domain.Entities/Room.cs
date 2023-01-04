using System.ComponentModel.DataAnnotations;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class Room : Entity
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public virtual ICollection<AccountRoom> Members { get; set; }
}