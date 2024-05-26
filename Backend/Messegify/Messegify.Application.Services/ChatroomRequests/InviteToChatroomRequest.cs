using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.ChatroomRequests;

public class InviteToChatroomRequest : IRequest
{
    public Guid ChatroomId { get; set; }
    public Guid AccountId { get; set; }

    public InviteToChatroomRequest(ChatroomInvite chatroomInvite)
    {
        ChatroomId = chatroomInvite.ChatroomId;
        AccountId = chatroomInvite.AccountId;
    }
}