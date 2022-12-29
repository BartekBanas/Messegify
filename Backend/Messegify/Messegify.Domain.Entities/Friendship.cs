﻿using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class Friendship : IEntity
{
    [ForeignKey(nameof(FirstAccount))]
    public Guid FirstAccountId { get; set; }
    public virtual Account FirstAccount { get; set; }
    
    [ForeignKey(nameof(SecondAccount))]
    public Guid SecondAccountId { get; set; }
    public virtual Account SecondAccount { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateCreated { get; set; }
}