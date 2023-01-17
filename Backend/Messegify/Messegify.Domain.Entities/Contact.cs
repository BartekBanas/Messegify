using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class Contact : Entity
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(FirstAccount))]
    public Guid FirstAccountId { get; set; }
    public virtual Account FirstAccount { get; set; } = null!;

    [ForeignKey(nameof(SecondAccount))]
    public Guid SecondAccountId { get; set; }
    public virtual Account SecondAccount { get; set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateCreated { get; set; }
}