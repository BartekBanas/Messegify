﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class Message : Entity
{
    [Key]
    public Guid Id { get; set; }

    public string TextContent { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime SentDate { get; set; }

    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; }
    
    [ForeignKey(nameof(ChatRoom))]
    public Guid ChatRoomId { get; set; }
    public virtual ChatRoom ChatRoom { get; set; }
}