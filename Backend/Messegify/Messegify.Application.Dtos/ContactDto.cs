namespace Messegify.Application.Dtos;

public class ContactDto
{
    public Guid Id { get; set; }
    
    public Guid FirstAccountId { get; set; }
    
    public Guid SecondAccountId { get; set; }
    
    public DateTime DateCreated { get; set; }
}