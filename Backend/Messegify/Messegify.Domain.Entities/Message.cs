using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Messegify.Domain.Abstractions;

namespace Messegify.Domain.Entities;

public class Message : Entity
{
    [Key]
    public Guid Id { get; set; }

    public string TextContent { get; set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime SentDate { get; set; }

    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; } = null!;

    [ForeignKey(nameof(Chatroom))]
    public Guid ChatRoomId { get; set; }
    public virtual Chatroom Chatroom { get; set; } = null!;
}