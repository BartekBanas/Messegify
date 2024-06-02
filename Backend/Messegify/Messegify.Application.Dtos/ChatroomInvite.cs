namespace Messegify.Application.Dtos;

public class ChatroomInvite
{
    public required Guid ChatroomId { get; set; }
    public required Guid AccountId { get; set; }
}