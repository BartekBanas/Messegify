namespace Messegify.Application.Dtos;

public class ContactDto
{
    public Guid Id { get; set; }
    
    public Guid FirstAccountId { get; set; }
    
    public Guid SecondAccountId { get; set; }

    public Guid ContactChatRoomId { get; set; }
    
    public DateTime DateCreated { get; set; }

    public bool Active { get; set; }
}